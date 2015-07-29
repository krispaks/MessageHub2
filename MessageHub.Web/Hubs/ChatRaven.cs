using MessageHub.Web.Controllers;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageHub.Web.Hubs
{
    public class ChatRaven
    {
        //DocumentStore documentStore = new DocumentStore {
        //        // db connection
        //        Url = "http://localhost:8080/",
        //        DefaultDatabase = "MessageHubDB"
        //    };
        //    documentStore.Initialize();

        public static void StoreMessage(string message, string strFrom, string strTo)
        {
            //string db = message + " " + strFrom + " " + strTo;

            //ChatMessageApiController obj = new ChatMessageApiController();
            //obj.MethodName();
            //int resp = ChatMessageApiController.Post();
        }

    }
}