using MessageHub.Lib.Entity;
using MessageHub.Lib.Utility;
using Raven.Abstractions.Commands;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Repository
{
    public class MessageRavenRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {

        private DocumentStore documentStore;
        private IDocumentSession session;

        public MessageRavenRepository()
        {
            documentStore = new DocumentStore
            {
                // db connection
                Url = "http://localhost:8080/",
                DefaultDatabase = "MessageHubDB"
            };
            documentStore.Initialize();        
        }


        public TEntity Get(int id)
        {
            using (session = documentStore.OpenSession())
            {
                var message = session.Load<TEntity>(id);
                return message;
            }
        }

        public IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            using (session = documentStore.OpenSession())
            {
                IQueryable<TEntity> query = session.Query<TEntity>();
                Filter(ref query, filter);
                IncludeProperties(ref query, includeProperties);
                return OrderBy(query, orderBy);
            }
        }

        public IEnumerable<TEntity> GetPaged(Utility.PagingInfo pageInfo, System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            using (session = documentStore.OpenSession())
            {
                IQueryable<TEntity> query = session.Query<TEntity>();
                Filter(ref query, filter);
                IncludeProperties(ref query, includeProperties);
                var orderedQuery = OrderBy(query, orderBy);
                return PagedData(orderedQuery, pageInfo);
            }
        }

        public void Insert(TEntity entity)
        {
            using (session = documentStore.OpenSession())
            {
                session.Store(entity);
            }
        }

        public void Delete(int id)
        {
            using (session = documentStore.OpenSession())
            {   
                var message = session.Load<TEntity>(id);
                session.Delete(message);
            }
            /*
             for using TEntity instead of the id, it would be something like this:
                session.Delete(message);
                session.SaveChanges();
             */

            /*
             for deleting without even loading the file, it would be like this:
                session.Advanced.Defer(new DeleteCommandData { Key = "messages/"+id });
                session.SaveChanges();
             */
        }

        public void Update(TEntity entity)
        {
            using (session = documentStore.OpenSession())
            {
                session.Store(entity);
            }
        }

        public int Save()
        {
            session.SaveChanges();
            return 1;
        }

        public void Dispose()
        {
            session.Dispose();
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
                return query;
            }
        }

        private IQueryable<TEntity> PagedData(IQueryable<TEntity> query, PagingInfo pageInfo)
        {
            pageInfo.TotalRecords = query.Count();
            pageInfo.TotalPages = (int)Math.Ceiling(pageInfo.TotalRecords / (float)pageInfo.Rows);

            return query.Skip((pageInfo.Page - 1) * pageInfo.Rows).Take(pageInfo.Rows);
        }

        #endregion
    }
}
