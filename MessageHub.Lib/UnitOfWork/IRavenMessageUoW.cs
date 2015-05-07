using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.Entity;
using MessageHub.Lib.Repository;
using Raven.Client;

namespace MessageHub.Lib.UnitOfWork
{
	public interface IRavenMessageUoW : IUnitOfWork
	{
		IRepository<Message, IDocumentSession> MessageRavenRepositoryRepository { get; set; }
        IRepository<Comment, IDocumentSession> CommentRavenRepositoryRepository { get; set; }
	}
}
