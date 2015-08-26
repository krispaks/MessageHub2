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
    public class RavenChatMessageUoW : IRavenChatMessageUoW
    {
        public IDocumentSession context;
        public static DocumentStore documentStore;
        public IChatRepository<ChatMessage, IDocumentSession> ChatMessageRavenRepositoryRepository { get; set; }

		public RavenChatMessageUoW()
		{
            // intialize db
            documentStore = new DocumentStore {
                // db connection
                Url = "http://localhost:8080/",
                DefaultDatabase = "MessageHubDB"
            };
            documentStore.Initialize();
            // allow multiple operations at the same time
            documentStore.Conventions.AllowMultipuleAsyncOperations = true;

            ChatMessageRavenRepositoryRepository = new ChatMessageRavenRepository<ChatMessage, DocumentSession> {
                Context = context
            };
		}

        public int SaveChanges()
        {
            int retValue = 0;
            try {
                using (context) {
                    retValue = this.ChatMessageRavenRepositoryRepository.Save();
                }
            } catch (Exception ex) {
            }
            return retValue;
        }

        public void Dispose()
        {
            this.ChatMessageRavenRepositoryRepository.Dispose();
        }
    }
}
