using MessageHub.Lib.DTO;
using MessageHub.Lib.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Service
{
    public class RavenMessageService : IMessageService
    {
        private IRavenMessageUoW _uow = null;
        private ILoggingService _logService = null;

        public RavenMessageService(IRavenMessageUoW uow, ILoggingService logService)
        {
            this._uow = uow;
            this._logService = logService;
        }

        public int SaveMessage(Entity.Message message)
        {
            try {
				int retValue = 0;
				this._logService.Log("Start SaveMessage");

				if (Validate(message)) {
					_uow.MessageRavenRepositoryRepository.Insert(message);
					_uow.SaveChanges();

					retValue = message.Id;
				} else {
					this._logService.Log("Error in Validation");
				}
				return retValue;
			} catch (Exception ex) {
				this._logService.Log(string.Format("Error at SaveMessage : {0}", ex.Message));
				throw;
			} finally {
				this._logService.Log("End SaveMessage");
			}
        }

        private bool Validate(Entity.Message message)
        {
            return true;
        }

        public IEnumerable<Entity.Message> GetMessageList()
        {
            try {
                this._logService.Log("Start GetMessageList");

                // gets all the messages from the db
                var messages = _uow.MessageRavenRepositoryRepository.Get();
                return messages;
                
            } catch (Exception ex) {
                this._logService.Log(string.Format("Error at GetMessageList : {0}", ex.Message));
                throw;
            } finally {
                this._logService.Log("End GetMessageList");
            }
        }

        public Entity.Message GetMessage(int id)
        {
            try {
                this._logService.Log("Start GetMessage");

                // gets a message from the db by its id
                var message = _uow.MessageRavenRepositoryRepository.Get(id);
                return message;

            } catch (Exception ex) {
                this._logService.Log(string.Format("Error at GetMessage : {0}", ex.Message));
                throw;
            } finally {
                this._logService.Log("End GetMessage");
            }
        }

        public IEnumerable<Entity.Comment> GetComments(int id)
        {
            try
            {
                this._logService.Log("Start GetComments");

                // gets all the comments for a specified message
                var comments = _uow.CommentRavenRepositoryRepository.Get(filter: x => x.MessageId == id);
                return comments;

            }
            catch (Exception ex)
            {
                this._logService.Log(string.Format("Error at GetComments : {0}", ex.Message));
                throw;
            }
            finally
            {
                this._logService.Log("End GetComments");
            }
        }

        public int UpdateMessage(Entity.Message message)
        {
            try
            {
                int retValue = 0;
                this._logService.Log("Start UpdateMessage");

                if (Validate(message))
                {
                    // updates the content of a message in the db
                    _uow.MessageRavenRepositoryRepository.Update(message);
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
                this._logService.Log(string.Format("Error at UpdateMessage : {0}", ex.Message));
                throw;
            }
            finally
            {
                this._logService.Log("End UpdateMessage");
            }
        }

        public void DeleteMessage(int id)
        {
            try {
                this._logService.Log("Start DeleteMessage");

                // deletes the message from the db
                _uow.MessageRavenRepositoryRepository.Delete(id);
                _uow.SaveChanges();

            } catch (Exception ex) {
                this._logService.Log(string.Format("Error at DeleteMessage : {0}", ex.Message));
                throw;
            } finally {
                this._logService.Log("End DeleteMessage");
            }
        }


		public DTO.MessageDetailDTO GetMessageDetail(int id)
		{
            try
            {
                this._logService.Log("Start GetMessageDetail");
                MessageDetailDTO dto = new MessageDetailDTO
                {
                    MessageInfo = _uow.MessageRavenRepositoryRepository.Get(id),
                    CommentList = _uow.CommentRavenRepositoryRepository.Get(filter: x => x.MessageId == id)
                };

                return dto;
            }
            catch (Exception ex)
            {
                this._logService.Log(string.Format("Error at GetMessageDetail : {0}", ex.Message));
                throw;
            }
            finally
            {
                this._logService.Log("End GetMessageDetail");
            }
		}
		
		public PagedResultDTO<Entity.Message> GetPagedMessageList(MessageSearchCriteriaDTO searchCriteria)
		{
            try
            {
                this._logService.Log("Start GetPagedMessageList");

                var paged = _uow.MessageRavenRepositoryRepository.GetPaged(
                    searchCriteria.PagingInfo,
                    // passes the lambda expression for each of the fields and the content of the field entered by the user
                    filterTitleExpression: x => x.Title,
                    filterTitleField: searchCriteria.Title,
                    filterSubCategoryExpression: x => x.SubCategoryId,
                    filterSubCategoryField: ""+searchCriteria.SubCategory,
                    filterTagsExpression: x => x.Tags,
                    filterTagsField: searchCriteria.Tag);
                
                return paged;
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
	}
}
