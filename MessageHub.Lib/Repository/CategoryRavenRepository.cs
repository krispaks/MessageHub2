﻿using MessageHub.Lib.Entity;
using MessageHub.Lib.UnitOfWork;
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
using MessageHub.Lib.DTO;

namespace MessageHub.Lib.Repository
{
    public class CategoryRavenRepository<TEntity, TContext> : IRepository<TEntity, IDocumentSession>
        where TEntity : BaseEntity
    {

        //private DocumentStore documentStore;
        private IDocumentSession session;

        public CategoryRavenRepository()
        {
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
            var category = session.Load<TEntity>(id);
            return category;
        }

        public IEnumerable<TEntity> Get()
        {
            IQueryable<TEntity> query = null;

            // open a new session on the documentStore defined at the UoW
            session = RavenCategoryUoW.documentStore.OpenSession();

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

            return query;
        }

        public IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = null;

            // open a new session on the documentStore defined at the UoW
            session = RavenMessageUoW.documentStore.OpenSession();

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

            return query;
        }

        public PagedResultDTO<TEntity> GetPaged(PagingInfoDTO pageInfo, System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            return null;
        }
        
        public void Insert(TEntity entity)
        {
            // open a new session on the documentStore defined at the UoW
            session = RavenCategoryUoW.documentStore.OpenSession();
            session.Store(entity);
        }

        public void Delete(int id)
        {
            // open a new session on the documentStore defined at the UoW
            session = RavenCategoryUoW.documentStore.OpenSession();
            var category = session.Load<TEntity>(id);
            session.Delete(category);
        }

        public void Update(TEntity entity)
        {
            // open a new session on the documentStore defined at the UoW
            session = RavenCategoryUoW.documentStore.OpenSession();
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

        private IQueryable<TEntity> PagedData(IQueryable<TEntity> query, PagingInfoDTO pageInfo)
        {
            pageInfo.TotalRecords = query.Count();
            pageInfo.TotalPages = (int)Math.Ceiling(pageInfo.TotalRecords / (float)pageInfo.Rows);

            return query.Skip((pageInfo.Page - 1) * pageInfo.Rows).Take(pageInfo.Rows);
        }

        #endregion
	}
}
