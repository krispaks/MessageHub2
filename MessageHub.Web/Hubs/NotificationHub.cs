using MessageHub.Lib.DTO;
using MessageHub.Lib.Entity;
using MessageHub.Lib.Repository;
using MessageHub.Lib.Service;
using MessageHub.Lib.UnitOfWork;
using MessageHub.Web.Controllers;
using Microsoft.AspNet.SignalR;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MessageHub.Web.Hubs
{
    public class NotificationHub : Hub
    {
        public static string notificationList = null;

        public override Task OnConnected()
        {
            List<string[]> lista = new List<string[]>();
            lista = NotificationQuery();

            Clients.Caller.notificationList(lista);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public void updateWithNewNotification()
        {
            List<string[]> lista = new List<string[]>();
            lista = NotificationQuery();

            // update the list of notifications for all the clients connected
            Clients.All.updateNotificationList(lista);
        }

        public List<string[]> NotificationQuery()
        {
            IRavenMessageUoW uow = new RavenMessageUoW();
            ILoggingService logger = new LoggingService();
            IMessageService messageService = new RavenMessageService(uow, logger);
            ICommentService commentService = new RavenCommentService(uow.CommentRavenRepositoryRepository, logger);

            UserInfoApiController cont = new UserInfoApiController();

            // loads both the messages and the comments
            var messageList = messageService.GetMessageList().OrderByDescending(item => item.CreatedDate).Take(5);
            var commentList = commentService.GetAllComments().OrderByDescending(item => item.CreatedDate).Take(5);

            // stores them in a common list
            List<string[]> lista = new List<string[]>();
            var userName = new string[] {};
            try
            {
                foreach (var item in messageList)
                {
                    userName = cont.GetUserRealName(item.CreatedBy);
                    lista.Add(new string[] { item.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"), userName[1] + " " + userName[2], item.Title, "" + item.Id, "message" });
                }
                foreach (var item in commentList)
                {
                    userName = cont.GetUserRealName(item.CreatedBy);
                    lista.Add(new string[] { item.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"), userName[1] + " " + userName[2], "" + messageService.GetMessage(item.MessageId).Title, "" + item.MessageId, "comment" });
                }
            }
            catch (Exception ex) { }

            // chops the list to show only the 5 newer ones
            lista = lista.OrderByDescending(item => item[0]).Take(5).ToList();

            return lista;
        }
    }
}