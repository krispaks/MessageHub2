using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.Repository;
using MessageHub.Lib.Entity;
using Raven.Client;

namespace MessageHub.Lib.UnitOfWork
{
	public interface IMessageUoW : IUnitOfWork
	{
		IRepository<Message, MessageHubDbContext> MessageHubRepositoryRepository { get; set; }
	}

    public interface IRavenMessageUoW : IUnitOfWork
    {
        IRepository<Message, IDocumentSession> MessageRavenRepositoryRepository { get; set; }
    }
}
