using System;
using MessageHub.Lib.Repository;
using MessageHub.Lib.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Xunit;
//using Xunit.Sdk;
using MessageHub.Lib.Utility;
using Moq;
using MessageHub.Lib.Entity;
using MessageHub.Lib.Service;

namespace MessageHub.Test
{	
	[TestClass]
	public class MessageServiceIntegrationTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			IMessageUoW msgUow = new MessageUoW();
			ILoggingService logService = new LoggingService();
			MessageService msgService = new MessageService(msgUow, logService);

			var id = msgService.SaveMessage(new Message
			{
				Title = "Test",
				Content = "Test",
				SubCategoryId = 2,
				CreatedBy = "mail@mail.com",
				CreatedDate = UtilityDate.HubDateTime()
			});

			Assert.IsTrue(id>0);
		}

		[TestMethod]
		public void TestMethod2()
		{
			MessageHubRepository<Message> message = new MessageHubRepository<Message>();
			message.Insert(new Message
			{
				Title = "Test",
				Content = "Test",
				SubCategoryId = 2,
				CreatedBy = "mail@mail.com",
				CreatedDate = UtilityDate.HubDateTime()
			});

			message.Save();
		} 
	}
}
