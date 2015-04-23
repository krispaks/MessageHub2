using MessageHub.Lib.Entity;
using MessageHub.Lib.Repository;
using MessageHub.Lib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.UnitOfWork
{
    public class RavenMessageUoW : IMessageUoW
    {
        public IRepository<Message> MessageHubRepositoryRepository { get; set; }

		public RavenMessageUoW()
		{
            MessageHubRepositoryRepository = new MessageRavenRepository<Message>();
		}

        public int SaveChanges()
        {
            int retValue = 0;
            try {
                retValue = this.MessageHubRepositoryRepository.Save();
            } catch (Exception ex) {

            }
            return retValue;
        }

        public void Dispose()
        {
            this.MessageHubRepositoryRepository.Dispose();
        }

    }
}
