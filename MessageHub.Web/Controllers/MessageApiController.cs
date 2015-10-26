using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MessageHub.Lib.Service;
using MessageHub.Lib.UnitOfWork;
using MessageHub.Lib.Utility;
using MessageHub.Web.Models;
using MessageHub.Lib.Entity;
using Newtonsoft.Json.Linq;
using MessageHub.Lib.DTO;
using System.Web;
using System.IO;
using Raven.Json.Linq;
using Raven.Client.FileSystem;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace MessageHub.Web.Controllers
{
    [Authorize]
    public class MessageApiController : ApiController
    {
        private IMessageService messageService = null;
        private ILoggingService logger = null;
        private UserInfoApiController userInfo = null;

        public MessageApiController(IMessageService messageService, ILoggingService logger)
        {
            this.messageService = messageService;
            this.logger = logger;
        }

        public HttpResponseMessage Get([FromUri] MessageSearchCriteriaViewModel searchCriteria)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var searchCriteriaDTO = new MessageSearchCriteriaDTO
                {
                    PagingInfo = new PagingInfoDTO
                    {
                        Page = searchCriteria.Page,
                        Rows = searchCriteria.Rows
                    },
                    Title = searchCriteria.Title,
                    Tag = searchCriteria.Tag,
                    SubCategory = searchCriteria.SubCategory
                };

                var pagedResult = this.messageService.GetPagedMessageList(searchCriteriaDTO);

                userInfo = new UserInfoApiController();

                List<MessageListViewModel> searchResult = pagedResult.Data.ToList().Select(message => new MessageListViewModel
                {
                    Id = message.Id,
                    Title = message.Title,
                    ContentConcat = message.Content.Length <= 192 ? message.Content : message.Content.Substring(0, 192) + "...",
                    //ContentConcat = message.Content,
                    //CreatedBy = message.CreatedBy,
                    CreatedBy = userInfo.GetUserRealName(message.CreatedBy)[1] + " " + userInfo.GetUserRealName(message.CreatedBy)[2],
                    CreatedDate = UtilityDate.HubDateString(message.CreatedDate)
                }).ToList();

                var vm = new PagedResultViewModel<MessageListViewModel>
                {
                    Data = searchResult,
                    PageInfo = new PageInfoViewModel
                    {
                        Page = pagedResult.PagingInfo.Page,
                        Rows = pagedResult.PagingInfo.Rows,
                        TotalPages = pagedResult.PagingInfo.TotalPages,
                        TotalRecords = pagedResult.PagingInfo.TotalRecords
                    }
                };

                response = Request.CreateResponse(HttpStatusCode.OK, vm);
            }
            catch (Exception ex)
            {
                this.logger.Log(string.Format("Error at Message Get : {0}", ex.Message));
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }

        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {

                var message = this.messageService.GetMessage(id);

                userInfo = new UserInfoApiController();

                var vm = new MessageDetailViewModel
                {
                    Message = new MessageViewModel
                    {
                        Id = message.Id,
                        Title = message.Title,
                        Content = message.Content,
                        Tags = message.Tags,
                        CreatedBy = userInfo.GetUserRealName(message.CreatedBy)[1] + " " + userInfo.GetUserRealName(message.CreatedBy)[2],
                        CreatedDate = UtilityDate.HubDateString(message.CreatedDate)
                    }
                };

                response = Request.CreateResponse(HttpStatusCode.OK, vm);
            }
            catch (Exception ex)
            {
                this.logger.Log(string.Format("Error at Message Get : {0}", ex.Message));
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }

        public HttpResponseMessage Post(/*MessageViewModel newMessage*/)
        {
            // request the data sent from the client
            var httpRequest = HttpContext.Current.Request;
            // request the content for the message
            var newMessage = httpRequest["newMessage"];
            // request the file attached by the user
            Stream uploadStream = null;
            var fileName = "";
            if (httpRequest.Files["UploadedFile"] != null)
            {
                uploadStream = httpRequest.Files["UploadedFile"].InputStream;
                fileName = httpRequest["FileName"];

            }

            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                if (newMessage != null)
                {
                    // deserialize the json an re-build the message
                    JObject newMessageObj = JObject.Parse(newMessage);
                    int subCategoryId = -1;
                    JToken outValue;
                    if (newMessageObj.TryGetValue("SubCategoryId", out outValue))
                        subCategoryId = (int)outValue;

                    Message message = new Message
                    {
                        Title = newMessageObj.Property("Title").Value.ToString(),
                        Content = newMessageObj.Property("Content").Value.ToString(),
                        SubCategoryId = subCategoryId,
                        Tags = newMessageObj.Property("Tags").Value.ToString().ToLower(),
                        CreatedBy = User.Identity.Name,
                        CreatedDate = UtilityDate.HubDateTime()
                    };

                    // saves the message in the db
                    int messageId = this.messageService.SaveMessage(message);

                    // if the user has uploaded a file
                    if (uploadStream != null)
                    {
                        // get the info and metadata for the file
                        string fileId = messageId.ToString()+"_"+httpRequest.Files[0].FileName;
                        var metadata = new RavenJObject {
                            {"Id", fileId},
                            {"Name", fileName},
                            {"Message", messageId.ToString()}
                        };

                        // connects to the service to store the file in the db
                        this.messageService.StoreFiles(uploadStream, fileId, metadata);
                    }
                    
                    // generates the response to return to the ui
                    response = Request.CreateResponse(HttpStatusCode.OK, messageId);
                }
            }
            catch (Exception ex)
            {
                this.logger.Log(string.Format("Error at Message Get : {0}", ex.Message));
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }

        public async Task<System.Net.Http.HttpResponseMessage> GetFile(string fileId, bool download)
        {
            FilesStore filesStore = new FilesStore() {
                Url = "http://localhost:8080/",
                DefaultFileSystem = "MessageHubDB"
            };
            filesStore.Initialize();
            var session = filesStore.OpenAsyncSession();

            try
            {
                // if the user is trying to download the file
                if (download == true)
                {
                    var stream = session.DownloadAsync("files/" + fileId + "_blob");
                    stream.Wait();
                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                    result.Content = new StreamContent(await stream);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                    return result;
                }
                // if the user is checking if there's attached files to the message
                else {
                    var query = await session.Query()
                                 .WhereEquals("Message", fileId)
                                 .ToListAsync();

                    RavenJToken realName;
                    string realNameString = "";
                    if (query.FirstOrDefault().Metadata.TryGetValue("Name", out realName))
                        realNameString = ""+realName;

                    return Request.CreateResponse(HttpStatusCode.OK, realName);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}