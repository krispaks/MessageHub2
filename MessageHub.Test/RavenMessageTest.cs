using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageHub.Lib.Entity;
using MessageHub.Lib.Repository;
using MessageHub.Lib.Utility;

namespace MessageHub.Test
{
    [TestClass]
    public class RavenMessageTest
    {
        [TestMethod]
        public void TestGet()
        {
            MessageRavenRepository<Message> repository = new MessageRavenRepository<Message>();
            var item = repository.Get(33);
            int id = item.Id;
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
        }
    }
}
