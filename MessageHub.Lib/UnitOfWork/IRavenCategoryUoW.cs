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
	public interface IRavenCategoryUoW : IUnitOfWork
	{
		IRepository<Category, IDocumentSession> CategoryRavenRepositoryRepository { get; set; }
	}
}
