using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.DTO;
using MessageHub.Lib.UnitOfWork;
using MessageHub.Lib.Entity;

namespace MessageHub.Lib.Service
{
	public class MessageService : IMessageService
	{
		private IMessageUoW _uow = null;
		private ILoggingService _logService = null;

		public MessageService()
		{
		}

		public MessageService(IMessageUoW uow, ILoggingService logService)
		{
			this._uow = uow;
			this._logService = logService;
		}

		public int SaveMessage(Message message)
		{
			try
			{
				int retValue = 0;
				this._logService.Log("Start Save Message");

				if (Validate(message))
				{
					_uow.MessageHubRepository.Insert(message);
					_uow.SaveChanges();

					retValue = message.Id;
				}
				else
				{
					this._logService.Log("Error in Validation");
				}

				return retValue;
			}
			catch (Exception ex)
			{
				this._logService.Log(string.Format("Error at SaveMessage : {0}", ex.Message));
				throw;
			}
			finally
			{
				this._logService.Log("End Save Message");
			}
		}

		public IEnumerable<Message> GetMessageList()
		{
			try
			{
				this._logService.Log("Start GetMessageList");

				return _uow.MessageHubRepository.Get();
			}
			catch (Exception ex)
			{
				this._logService.Log(string.Format("Error at GetMessageList : {0}", ex.Message));
				throw;
			}
			finally
			{
				this._logService.Log("End GetMessageList");
			}
		}

		public IEnumerable<Message> GetPagedMessageList(MessageSearchCriteriaDTO searchCriteria)
		{
			try
			{
				this._logService.Log("Start GetPagedMessageList");

				return _uow.MessageHubRepository.GetPaged(searchCriteria.PagingInfo, 
					filter: x=> x.Title == searchCriteria.Title 
						|| x.SubCategoryId == searchCriteria.SubCategory);
			}
			catch (Exception ex)
			{
				this._logService.Log(string.Format("Error at GetPagedMessageList : {0}", ex.Message));
				throw;
			}
			finally
			{
				this._logService.Log("End GetPagedMessageList");
			}
		}

        public Message GetMessage(int id)
        {
            try {
                this._logService.Log("Start GetMessage");

                return _uow.MessageHubRepository.Get(id);
            } catch (Exception ex) {
                this._logService.Log(string.Format("Error at GetMessage : {0}", ex.Message));
                throw;
            } finally {
                this._logService.Log("End GetMessage");
            }
        }

		public MessageDetailDTO GetMessageDetail(int id)
		{
			try
			{
				this._logService.Log("Start GetMessage");

				MessageDetailDTO dto = new MessageDetailDTO
				{
					MessageInfo = _uow.MessageHubRepository.Get(id),
					CommentList = _uow.CommentHubRepository.Get(filter: x => x.MessageId == id)
				};

				return dto;
			}
			catch (Exception ex)
			{
				this._logService.Log(string.Format("Error at GetMessage : {0}", ex.Message));
				throw;
			}
			finally
			{
				this._logService.Log("End GetMessage");
			}
		}

		private bool Validate(Message message)
		{
			return true;
		}
		
	}
}
