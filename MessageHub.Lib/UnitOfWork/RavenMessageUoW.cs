using MessageHub.Lib.Entity;
using MessageHub.Lib.Repository;
using MessageHub.Lib.Utility;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.UnitOfWork
{
    public class RavenMessageUoW : IRavenMessageUoW
    {
        //public IRepository<Message, MessageHubDbContext> MessageHubRepositoryRepository { get; set; }
        //public MessageRavenRepository<Message, IDocumentSession> MessageHubRepositoryRepository { get; set; }
        public IDocumentSession context;
        public IRepository<Message, IDocumentSession> MessageRavenRepositoryRepository { get; set; }

		public RavenMessageUoW()
		{
            // intialize db
            DocumentStore documentStore = new DocumentStore {
                // db connection
                Url = "http://localhost:8080/",
                DefaultDatabase = "MessageHubDB"
            };
            documentStore.Initialize();

            // initialize session
            context = documentStore.OpenSession();

            MessageRavenRepositoryRepository = new MessageRavenRepository<Message, DocumentSession> {
                Context = context
            };
		}

        public int SaveChanges()
        {
            int retValue = 0;
            try {
                //retValue = this.repository.Save();
                using (context) {
                    retValue = this.MessageRavenRepositoryRepository.Save();
                }
            } catch (Exception ex) {
            }
            return retValue;
        }

        public void Dispose()
        {
            this.MessageRavenRepositoryRepository.Dispose();
        }
    }
}
