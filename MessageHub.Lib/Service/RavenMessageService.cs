﻿using MessageHub.Lib.UnitOfWork;
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

                // prueba de funcionamiento
                /*foreach (var mess in messages){
                    int ident = mess.Id;
                }*/

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

                // gets all the messages from the db
                var message = _uow.MessageRavenRepositoryRepository.Get(id);
                return message;

            } catch (Exception ex) {
                this._logService.Log(string.Format("Error at GetMessage : {0}", ex.Message));
                throw;
            } finally {
                this._logService.Log("End GetMessage");
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
			throw new NotImplementedException();
		}
	}
}
