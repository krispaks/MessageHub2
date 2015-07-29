using MessageHub.Lib.DTO;
using MessageHub.Lib.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Service
{
    public class RavenChatMessageService : IChatMessageService
    {
        private IRavenChatMessageUoW _uow = null;
        private ILoggingService _logService = null;

        public RavenChatMessageService(IRavenChatMessageUoW uow, ILoggingService logService)
        {
            this._uow = uow;
            this._logService = logService;
        }

        public int SaveChatMessage(Entity.ChatMessage chatMessage)
        {
            try {
				int retValue = 0;
				this._logService.Log("Start SaveChatMessage");

				_uow.ChatMessageRavenRepositoryRepository.Insert(chatMessage);
				_uow.SaveChanges();

				retValue = chatMessage.Id;

				return retValue;
			} catch (Exception ex) {
				this._logService.Log(string.Format("Error at SaveChatMessage : {0}", ex.Message));
				throw;
			} finally {
				this._logService.Log("End SaveChatMessage");
			}
        }
	}
}
