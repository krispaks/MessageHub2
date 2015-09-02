using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageHub.Lib.Entity;
using MessageHub.Lib.Repository;
using MessageHub.Lib.Utility;
using Raven.Client.Document;
using Raven.Client;
using MessageHub.Lib.UnitOfWork;
using MessageHub.Lib.Service;

namespace MessageHub.Test
{
    [TestClass]
    public class RavenMessageTest
    {

        [TestMethod]
        public void TestGetAllRaven()
        {
            IRavenMessageUoW msgUow = new RavenMessageUoW();
            ILoggingService logService = new LoggingService();

            RavenMessageService msgService = new RavenMessageService(msgUow, logService);
            var messages = msgService.GetMessageList();

            // now let's check if there's somethin' in there
            foreach(var mess in messages){
                int messId = mess.Id;
            }
        }

        [TestMethod]
        public void TestGetRaven()
        {
            IRavenMessageUoW msgUow = new RavenMessageUoW();
            ILoggingService logService = new LoggingService();

            RavenMessageService msgService = new RavenMessageService(msgUow, logService);
            var message = msgService.GetMessage(193);

            // now let's check if there's somethin' in there
            var somethin = message.Title;
        }

        [TestMethod]
        public void TestSaveRaven()
        {
            IRavenMessageUoW msgUow = new RavenMessageUoW();
            ILoggingService logService = new LoggingService();
            RavenMessageService msgService = new RavenMessageService(msgUow, logService);

            var id = msgService.SaveMessage(new Message
            {
                Title = "Test Service UoW",
                Content = "Test Service UoW",
                SubCategoryId = 1,
                CreatedBy = "mail@mail.com",
                CreatedDate = UtilityDate.HubDateTime()
            });
        }

        [TestMethod]
        public void TestUpdateRaven()
        {
            IRavenMessageUoW msgUow = new RavenMessageUoW();
            ILoggingService logService = new LoggingService();
            RavenMessageService msgService = new RavenMessageService(msgUow, logService);

            // load the original message
            var origMsg = msgService.GetMessage(449);

            // updates the content of the message
            origMsg.Title = "Test-Mod UoW";
            origMsg.Content = "Test-Mod UoW";
            origMsg.ModifiedBy = "mail@mail.com";
            origMsg.ModifiedDate = UtilityDate.HubDateTime();

            // saves it to the db
            var id = msgService.UpdateMessage(origMsg);
        }

        [TestMethod]
        public void TestDeleteRaven()
        {
            IRavenMessageUoW msgUow = new RavenMessageUoW();
            ILoggingService logService = new LoggingService();

            RavenMessageService msgService = new RavenMessageService(msgUow, logService);
            msgService.DeleteMessage(481);
        }

        [TestMethod]
        public void TestSaveCommentRaven()
        {
            IRavenMessageUoW msgUow = new RavenMessageUoW();
            IRepository<Comment, IDocumentSession> commentRepository = msgUow.CommentRavenRepositoryRepository;
            ILoggingService logService = new LoggingService();
            RavenCommentService cmtService = new RavenCommentService(commentRepository, logService);

            /*
                PARA RELLENAR EL NULL PRUEBA CON SOMETHIN LIKE THIS:
                RavenMessageService msgService = new RavenMessageService(msgUow, logService);
                var message = msgService.GetMessage(193);
             */

            var id = cmtService.SaveComment(new Comment
            {
                MessageId = 193,
                Value = "Just another test comment",
                Message = null,
                CreatedBy = "mail@mail.com",
                CreatedDate = UtilityDate.HubDateTime()
            });
        }

        /*[TestMethod]
        public void TestGet()
        {
            // intialize db
            DocumentStore documentStore = new DocumentStore
            {
                // db connection
                Url = "http://localhost:8080/",
                DefaultDatabase = "MessageHubDB"
            };
            documentStore.Initialize();

            // initialize session
            IDocumentSession session = documentStore.OpenSession();

            MessageRavenRepository<Message, IDocumentSession> repository = new MessageRavenRepository<Message, IDocumentSession>();

            using (session)
            {
                var item = repository.Get(33);
                int id = item.Id;
            }
        }

        [TestMethod]
        public void TestInsert()
        {
            MessageRavenRepository<Message> repository = new MessageRavenRepository<Message>();
            var mess = new Message
            {
                Title = "Test3",
                Content = "Test3",
                SubCategoryId = 3,
                CreatedBy = 3,
                CreatedDate = UtilityDate.HubDateTime()
            };

            repository.Insert(mess);
            repository.Save();
        }

        [TestMethod]
        public void TestDelete()
        {
            MessageRavenRepository<Message> repository = new MessageRavenRepository<Message>();
            repository.Delete(129);
            repository.Save();
        }

        [TestMethod]
        public void TestUpdate()
        {
            MessageRavenRepository<Message> repository = new MessageRavenRepository<Message>();

            var origMsg = repository.Get(385);
            origMsg.Title = "Test-Mod";
            origMsg.Content = "Test-Mod";
            origMsg.ModifiedBy = 7;
            origMsg.ModifiedDate = UtilityDate.HubDateTime();

            repository.Update(origMsg);
            repository.Save();
        }

        [TestMethod]
        public void TestGetIEnum()
        {
            MessageRavenRepository<Message> repository = new MessageRavenRepository<Message>();
            var items = repository.Get(null, null, "");

            // test that it's got somethin
            foreach (var item in items){
                int id = item.Id;
            }
        }*/
    }
}
