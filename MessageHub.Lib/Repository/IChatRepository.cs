using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.DTO;
using MessageHub.Lib.Entity;

namespace MessageHub.Lib.Repository
{
	public interface IChatRepository<TEntity, TContext> : IDisposable
		where TEntity : BaseChatEntity
		where TContext : class 
	{
		TContext Context { get; set; }

		/// <summary>
		/// Get a single entity
		/// </summary>
		TEntity Get(int id);

		/// <summary>
		/// Gets multiple entity based on filter and ordering by property
		/// </summary>
		IEnumerable<TEntity> Get();

		/// <summary>
		/// Save Data in DataStore
		/// </summary>
		void Insert(TEntity entity);

		/// <summary>
		/// Remove Data in DataStore
		/// </summary>
		void Delete(int id);

		/// <summary>
		/// Update the record in DataStore based on changes
		/// </summary>
		void Update(TEntity entity);

		/// <summary>
		/// Calls Save changes of Context
		/// </summary>
		int Save();
	}
}
