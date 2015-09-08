using MessageHub.Web.Models;
using MessageHub.Lib.Service;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Http;
using MessageHub.Lib.Entity;
using MessageHub.Lib.Utility;
using System.Web.Mvc;

namespace MessageHub.Web.Controllers
{
    
    public class ChatMessageApiController : ApiController
    {
        private IChatMessageService chatMessageService = null;
		private ILoggingService logger = null;

		public ChatMessageApiController(IChatMessageService chatMessageService, ILoggingService logger)
		{
			this.chatMessageService = chatMessageService;
			this.logger = logger;
		}

        public ActionResult SaveChatMsg(List<String> json)
        {
            ChatMessage message = new ChatMessage
            {
                From = json.ElementAt(0),
                To = json.ElementAt(1),
                Content = json.ElementAt(2),
                Time = UtilityDate.HubDateString(UtilityDate.HubDateTime())
            };

            int value = this.chatMessageService.SaveChatMessage(message);

            return null;
        }

        public IEnumerable<ChatMessage> GetChatMsgList(string from, string to)
        {
            var msglist = this.chatMessageService.GetChatMessageList(from, to);

            return msglist;
        }
    }
}