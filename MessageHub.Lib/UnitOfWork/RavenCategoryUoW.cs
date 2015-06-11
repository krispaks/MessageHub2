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
    public class RavenCategoryUoW : IRavenCategoryUoW
    {
        public IDocumentSession context;
        public static DocumentStore documentStore;
        public IRepository<Category, IDocumentSession> CategoryRavenRepositoryRepository { get; set; }

		public RavenCategoryUoW()
		{
            // intialize db
            documentStore = new DocumentStore {
                // db connection
                Url = "http://localhost:8080/",
                DefaultDatabase = "MessageHubDB"
            };
            documentStore.Initialize();

            // initialize session
            CategoryRavenRepositoryRepository = new CategoryRavenRepository<Category, DocumentSession> {
                Context = context
            };
		}

        public int SaveChanges()
        {
            int retValue = 0;
            try {
                //retValue = this.repository.Save();
                using (context) {
                    retValue = this.CategoryRavenRepositoryRepository.Save();
                }
            } catch (Exception ex) {
            }
            return retValue;
        }

        public void Dispose()
        {
            this.CategoryRavenRepositoryRepository.Dispose();
        }
    }
}
