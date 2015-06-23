using MessageHub.Lib.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.Entity;
using MessageHub.Lib.DTO;

namespace MessageHub.Lib.Repository
{
	public class MessageHubRepository<TEntity> : IRepository<TEntity, MessageHubDbContext>
		where TEntity : BaseEntity
	{
		private MessageHubDbContext _context = null;
		private DbSet<TEntity> _dbSet = null;
		private bool disposed = false;

		public MessageHubDbContext Context
		{
			get { return _context; }
			set
			{
				_context = value;
				_dbSet = this._context.Set<TEntity>();
			}
		}

		public MessageHubRepository()
		{
			_context = new MessageHubDbContext();
			_dbSet = this._context.Set<TEntity>();
		}

		public TEntity Get(int id)
		{
			return this._dbSet.Find(id);
		}

		public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, 
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
			string includeProperties = "")
		{
			IQueryable<TEntity> query = this._dbSet;

			Filter(ref query, filter);

			IncludeProperties(ref query, includeProperties);

			return OrderBy(query, orderBy);
		}

		public PagedResultDTO<TEntity> GetPaged(PagingInfoDTO pageInfo, 
			Expression<Func<TEntity, bool>> filter = null, 
            //System.Linq.Expressions.Expression<System.Func<TEntity, object>> filter = null,
            //string filterField = "",
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = "")
		{
			IQueryable<TEntity> query = this._dbSet;

			//Filter(ref query, filter);

			IncludeProperties(ref query, includeProperties);

			var orderedQuery = OrderBy(query, orderBy);

			return new PagedResultDTO<TEntity>
			{
				Data = PagedData(orderedQuery, pageInfo),
				PagingInfo = pageInfo
			};
		}

        public PagedResultDTO<TEntity> GetPaged(
            PagingInfoDTO pageInfo,
            Expression<Func<TEntity, object>> filterTitleExpression = null,
            string filterTitleField = "",
            Expression<Func<TEntity, object>> filterSubCategoryExpression = null,
            string filterSubCategoryField = "",
            Expression<Func<TEntity, object>> filterTagsExpression = null,
            string filterTagsField = "",
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            return null;
        }

		public void Insert(TEntity entity)
		{
			this._dbSet.Add(entity);
		}

		public void Delete(int id)
		{
			TEntity entityToDelete = this._dbSet.Find(id);
			Delete(entityToDelete);
		}

		public void Update(TEntity entity)
		{
			this._dbSet.Attach(entity);
			this._context.Entry(entity).State = EntityState.Modified;
		}

		public int Save()
		{
			return this._context.SaveChanges();
		}

		#region -- Private Methods --

		private void Filter(ref IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter = null)
		{
			if (filter != null)
				query = query.Where(filter);
		}

		private void IncludeProperties(ref IQueryable<TEntity> query, string includeProperties = "")
		{
			foreach (var includeProperty in includeProperties.Split
				(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProperty);
			}
		}

		private IQueryable<TEntity> OrderBy(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
		{
			if (orderBy != null)
			{
				return orderBy(query);
			}
			else
			{
				return query.OrderByDescending(x=>x.Id);
			}
		}

		private IQueryable<TEntity> OrderBy(IQueryable<TEntity> query, string orderColumn, string direction)
		{
			var type = typeof(TEntity);
			var property = type.GetProperty(orderColumn);
			if (property == null)
				return query;

			var parameter = Expression.Parameter(type, "p");
			var propertyAccess = Expression.MakeMemberAccess(parameter, property);
			var orderByExpression = Expression.Lambda(propertyAccess, parameter);
			MethodCallExpression resultExp = null;
			if (string.IsNullOrEmpty(direction.Trim()) || direction.ToLower() == "asc")
				resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType },
				query.Expression, Expression.Quote(orderByExpression));
			else
				resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType },
				query.Expression, Expression.Quote(orderByExpression));

			return query.Provider.CreateQuery<TEntity>(resultExp);
		}

		private IQueryable<TEntity> PagedData(IQueryable<TEntity> query, PagingInfoDTO pageInfo)
		{

			pageInfo.TotalRecords = query.Count();
			pageInfo.TotalPages = (int)Math.Ceiling(pageInfo.TotalRecords / (float)pageInfo.Rows);

			return query.Skip((pageInfo.Page - 1) * pageInfo.Rows).Take(pageInfo.Rows);
		}

		private IQueryable<TEntity> PagedData(IQueryable<TEntity> query, int page, int row)
		{
			return query.Skip((page - 1) * row).Take(row);
		}


		private void Delete(TEntity entityToDelete)
		{
			if (this._context.Entry(entityToDelete).State == EntityState.Detached)
			{
				this._dbSet.Attach(entityToDelete);
			}
			this._dbSet.Remove(entityToDelete);
		}

		#endregion

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
