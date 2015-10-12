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

        public HttpResponseMessage Post(MessageViewModel newMessage)
        {

            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                Message message = new Message
                {
                    Title = newMessage.Title,
                    Content = newMessage.Content,
                    SubCategoryId = newMessage.SubCategoryId,
                    Tags = newMessage.Tags.ToLower(),
                    CreatedBy = User.Identity.Name,
                    CreatedDate = UtilityDate.HubDateTime()
                };

                int value = this.messageService.SaveMessage(message);

                response = Request.CreateResponse(HttpStatusCode.OK, value);
            }
            catch (Exception ex)
            {
                this.logger.Log(string.Format("Error at Message Get : {0}", ex.Message));
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}