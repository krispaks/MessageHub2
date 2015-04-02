using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.Repository;
using MessageHub.Lib.Entity;

namespace MessageHub.Lib.UnitOfWork
{
	public interface IMessageUoW : IUnitOfWork
	{
		IRepository<Message> MessageHubRepositoryRepository { get; set; }
	}
}
