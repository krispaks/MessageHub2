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

        //public int Post()
        //{
        //    ChatMessage message = new ChatMessage
        //    {
        //        From = "from",
        //        To = "to",
        //        Content = "content"
        //    };

        //    int value = this.chatMessageService.SaveChatMessage(message);

        //    return value;
        //}

        public ActionResult SomeAction(string pass)
        {
            ChatMessage chatMessage = new ChatMessage
            {
                From = "tim",
                To = "mat",
                Content = "omg"
            };

            int value = this.chatMessageService.SaveChatMessage(chatMessage);
            return null;
        }

        public ActionResult SaveChatMsg(List<String> json)
        {
            ChatMessage message = new ChatMessage
            {
                From = json.ElementAt(0),
                To = json.ElementAt(1),
                Content = json.ElementAt(2)
            };

            int value = this.chatMessageService.SaveChatMessage(message);

            return null;
        }
    }
}