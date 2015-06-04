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

		public MessageApiController(IMessageService messageService, ILoggingService logger)
		{
			this.messageService = messageService;
			this.logger = logger;
		}
		
		//public HttpResponseMessage Get()
		//{
		//	HttpResponseMessage response = new HttpResponseMessage();

		//	try
		//	{
		//		var messageList = this.messageService.GetMessageList();

		//		List<MessageListViewModel> searchResult = messageList.Select(message => new MessageListViewModel
		//		{
		//			Id = message.Id,
		//			Title = message.Title,
		//			ContentConcat = message.Content.Length <= 192 ? message.Content : message.Content.Substring(0, 192) + "...",
		//			//ContentConcat = message.Content,
		//			CreatedBy = "KPACA",
		//			CreatedDate = UtilityDate.HubDateString(message.CreatedDate)
		//		}).ToList();

		//		response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
		//	}
		//	catch (Exception ex)
		//	{
		//		this.logger.Log(string.Format("Error at Message Get : {0}", ex.Message));
		//		response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
		//	}

		//	return response;
		//}

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

				List<MessageListViewModel> searchResult = pagedResult.Data.ToList().Select(message => new MessageListViewModel
				{
					Id = message.Id,
					Title = message.Title,
					ContentConcat = message.Content.Length <= 192 ? message.Content : message.Content.Substring(0, 192) + "...",
					//ContentConcat = message.Content,
					CreatedBy = "KPACA",
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
				/*var messageDetail = this.messageService.GetMessageDetail(id);

				var vm = new MessageViewModel
				{
					Id = messageDetail.MessageInfo.Id,
					Title = messageDetail.MessageInfo.Title,
					Content = messageDetail.MessageInfo.Content,
					CreatedBy = "KPACA",
					CreatedDate = UtilityDate.HubDateString(messageDetail.MessageInfo.CreatedDate)
				};*/

                var message = this.messageService.GetMessage(id);

				//var vm = new MessageDetailViewModel
				//{
				//	Id = message.Id,
				//	Title = message.Title,
				//	Content = message.Content,
				//	CreatedBy = "KPACA",
				//	CreatedDate = UtilityDate.HubDateString(message.CreatedDate)
				//};

				var vm = new MessageDetailViewModel
				{
					Message = new MessageViewModel
					{
						Id = message.Id,
						Title = message.Title,
						Content = message.Content,
						CreatedBy = "KPACA",
						CreatedDate = UtilityDate.HubDateString(message.CreatedDate)
					}
				};

				/*vm.NewComment = new CommentViewModel
				{
					MessageId = messageDetail.MessageInfo.Id
				};

				vm.CommentList = new List<CommentViewModel>();

				foreach (var item in messageDetail.CommentList)
				{
					vm.CommentList.Add(new CommentViewModel
					{
						Id = item.Id,
						MessageId = item.MessageId,
						Value = item.Value,
						CreatedBy = "KPACA",
                        CreatedDate = UtilityDate.HubDateString(item.CreatedDate)
                        
					});
				}*/

				response = Request.CreateResponse(HttpStatusCode.OK, vm);
			}
			catch (Exception ex)
			{
				this.logger.Log(string.Format("Error at Message Get : {0}", ex.Message));
				response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
			}

			return response;
		}
		
        /* GET THINGS (ONLY FOR TESTING) */
		//public HttpResponseMessage GetThings(int page)
		//{
		//	HttpResponseMessage response = new HttpResponseMessage();
		//	PagingInfoDTO actualPage = new PagingInfoDTO();
            
		//	try
		//	{
		//		// gets the message list from the db
		//		var messageList = this.messageService.GetMessageList();

		//		// sets the info for the paging
		//		actualPage.Page = page;
		//		actualPage.Rows = 5;
		//		actualPage.TotalRecords = messageList.Count();
		//		actualPage.TotalPages = (actualPage.TotalRecords/actualPage.Rows);

		//		if (actualPage.TotalRecords % actualPage.Rows != 0)
		//			actualPage.TotalPages += 1;

		//		// generates the response for the client with the messages for the page passed
		//		List<MessageListViewModel> preSearchResult = messageList.Select(message => new MessageListViewModel
		//		{
		//			Id = message.Id,
		//			Title = message.Title,
		//			ContentConcat = message.Content.Length <= 192 ? message.Content : message.Content.Substring(0, 192) + "...",
		//			//ContentConcat = message.Content,
		//			CreatedBy = "KPACA",
		//			CreatedDate = UtilityDate.HubDateString(message.CreatedDate)
		//		}).ToList();

		//		List<MessageListViewModel> searchResult = new List<MessageListViewModel>();

		//		for (int i = 0; i < actualPage.Rows; i++)
		//		{
		//			if( (i+((actualPage.Page-1)*actualPage.Rows)) < actualPage.TotalRecords )
		//				searchResult.Add(preSearchResult.ElementAt(i+((actualPage.Page-1)*actualPage.Rows)));
		//		}

		//		response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
		//	}
		//	catch (Exception ex)
		//	{
		//		this.logger.Log(string.Format("Error at Message Get : {0}", ex.Message));
		//		response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
		//	}

		//	return response;
		//}
		
        public HttpResponseMessage Post(MessageDetailViewModel newMessage)
		{

            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                Message message = new Message
                {
                    Title = newMessage.Message.Title,
                    Content = newMessage.Message.Content,
                    SubCategoryId = 1,
                    CreatedBy = 1,
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
