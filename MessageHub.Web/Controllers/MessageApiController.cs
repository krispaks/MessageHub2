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

namespace MessageHub.Web.Controllers
{
	public class MessageApiController : ApiController
	{
		private IMessageUoW messageUoW = null;
		private ILoggingService logger = null;

		public MessageApiController(IMessageUoW messageUoW, ILoggingService logger)
		{
			this.messageUoW = messageUoW;
			this.logger = logger;
		}

		public HttpResponseMessage Get([FromUri] MessageSearchCriteria searchCriteria)
		{
			HttpResponseMessage response = new HttpResponseMessage();

			try
			{
				var messageList = this.messageUoW.MessageHubRepositoryRepository.Get();

				List<MessageListViewModel> searchResult = messageList.Select(message => new MessageListViewModel
				{
					Id = message.Id,
					Title = message.Title,
					ContentConcat = message.Content,
					CreatedBy = "KPACA",
					CreatedDate = UtilityDate.HubDateString(message.CreatedDate)
				}).ToList();

				response = Request.CreateResponse(HttpStatusCode.OK, searchResult);
			}
			catch (Exception ex)
			{
				this.logger.Log(string.Format("Error at Message Get : {0}", ex.Message));
				response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
			}

			return response;
		}

		// GET: api/Message/5
		public HttpResponseMessage Get(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();

			try
			{
				var message = this.messageUoW.MessageHubRepositoryRepository.Get(id);

				var vm = new MessageListViewModel
				{
					Id = message.Id,
					Title = message.Title,
					ContentConcat = message.Content,
					CreatedBy = "KPACA",
					CreatedDate = UtilityDate.HubDateString(message.CreatedDate)
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

		// POST: api/Message
		public void Post([FromBody]string value)
		{
		}

		// PUT: api/Message/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE: api/Message/5
		public void Delete(int id)
		{
		}
	}
}
