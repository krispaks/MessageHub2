using System;
using System.Text;
using System.Collections.Generic;
using MessageHub.Lib.Entity;
using MessageHub.Lib.Service;
using MessageHub.Lib.UnitOfWork;
using MessageHub.Lib.Utility;
using Moq;
using Xunit;

namespace MessageHub.Test
{
	/// <summary>
	/// Summary description for MessageServiceUnitTest
	/// </summary>
	public class MessageServiceUnitTest
	{
		Mock<IMessageUoW> messageUoW = new Mock<IMessageUoW>();
		Mock<ILoggingService> logginService = new Mock<ILoggingService>();

		public MessageServiceUnitTest()
		{
			
		}

		[Fact]
		public void MessageServiceSaveMessageSuccess()
		{
			//Arrange
			messageUoW.Setup(x => x.MessageHubRepository.Insert(It.IsAny<Message>()));
			messageUoW.Setup(x => x.SaveChanges()).Returns(1);
			logginService.Setup(x => x.Log(It.IsAny<string>())).Returns(true);
			Message message = new Message
			{
				Id = 1,
				Title = "Test",
				Content = "Test",
				SubCategoryId = 2,
				CreatedBy = 1,
				CreatedDate = UtilityDate.HubDateTime()
			};

			//Act
			MessageService msgService = new MessageService(messageUoW.Object, logginService.Object);
			var retValue = msgService.SaveMessage(message);

			//Assert
			Assert.True(retValue > 0);
		}

		[Fact]
		public void MesageServiceSaveMessageFailed()
		{
			//Arrange
			messageUoW.Setup(x => x.MessageHubRepository.Insert(It.IsAny<Message>()));
			messageUoW.Setup(x => x.SaveChanges()).Returns(0);
			logginService.Setup(x => x.Log(It.IsAny<string>())).Returns(true);
			Message message = new Message
			{
				Id = 0,
				Title = "Test",
				Content = "Test",
				SubCategoryId = 2,
				CreatedBy = 1,
				CreatedDate = UtilityDate.HubDateTime()
			};

			//Act
			MessageService msgService = new MessageService(messageUoW.Object, logginService.Object);
			var retValue = msgService.SaveMessage(message);

			//Assert
			Assert.True(retValue == 0);
		}
	}
}
