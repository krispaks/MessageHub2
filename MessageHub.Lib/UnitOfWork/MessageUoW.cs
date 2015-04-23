using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.Repository;
using MessageHub.Lib.Entity;


namespace MessageHub.Lib.UnitOfWork
{
	public class MessageUoW : IMessageUoW
	{	
		private MessageHubDbContext _context = null;
		private bool disposed = false;

		public IRepository<Message, MessageHubDbContext> MessageHubRepositoryRepository { get; set; }

		public MessageUoW()
		{	
			this._context = new MessageHubDbContext();
			MessageHubRepositoryRepository = new MessageHubRepository<Message>
			{
				Context = this._context
			};

		}

		public int SaveChanges()
		{
			using (var contextTransactionScope = this._context.Database.BeginTransaction())
			{
				int retValue = 0;
				try
				{
					retValue = this._context.SaveChanges();
					contextTransactionScope.Commit();
				}
				catch (Exception ex)
				{
					contextTransactionScope.Rollback();
				}
				return retValue;	
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
