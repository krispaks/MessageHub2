﻿using MessageHub.Lib.Entity;
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
    public class MessageRavenRepository<TEntity, TContext> : IRepository<TEntity, IDocumentSession>
        where TEntity : BaseEntity
    {

        private DocumentStore documentStore;
        private IDocumentSession session;

        public MessageRavenRepository()
        {
            // intialize db
            DocumentStore documentStore = new DocumentStore
            {
                // db connection
                Url = "http://localhost:8080/",
                DefaultDatabase = "MessageHubDB"
            };
            documentStore.Initialize();

            // initialize session
            session = documentStore.OpenSession();
        }

        public IDocumentSession Context
        {
            get
            {
                return this.session;
            }
            set 
            {
                this.session = value;
            }
        }


        public TEntity Get(int id)
        {
            var message = session.Load<TEntity>(id);
            return message;
        }

        public IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = null;

            int start = 0;
            while (true)
            {
                // each round of the loop we take 128 regs from the db
                IQueryable<TEntity> tempQuery = session.Query<TEntity>().Take(128).Skip(start);
                // when there's no more regs to take, we break the loop
                if (tempQuery.ToList().Count() == 0)
                    break;
                start += tempQuery.ToList().Count();

                // chorizo that concats new entries to the query each round of the loop
                query = query == null ? query = tempQuery : query = query.ToList().Concat(tempQuery.ToList()).ToList().AsQueryable();
            }

            Filter(ref query, filter);
            IncludeProperties(ref query, includeProperties);

            return OrderBy(query, orderBy);
        }

        public IEnumerable<TEntity> GetPaged(Utility.PagingInfo pageInfo, System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            //using (session = documentStore.OpenSession())
            //{
                IQueryable<TEntity> query = session.Query<TEntity>();
                Filter(ref query, filter);
                IncludeProperties(ref query, includeProperties);
                var orderedQuery = OrderBy(query, orderBy);
                return PagedData(orderedQuery, pageInfo);
            //}
        }

        public void Insert(TEntity entity)
        {
            session.Store(entity);
        }

        public void Delete(int id)
        {
            var message = session.Load<TEntity>(id);
            session.Delete(message);
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
            session.Store(entity);
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
