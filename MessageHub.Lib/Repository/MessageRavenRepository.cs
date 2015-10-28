using MessageHub.Lib.Entity;
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
using System.IO;
using Raven.Json.Linq;
using System.Net.Http;
using System.Net;

namespace MessageHub.Lib.Repository
{
    public class MessageRavenRepository<TEntity, TContext> : IRepository<TEntity, IDocumentSession>
        where TEntity : BaseEntity
    {

        //private DocumentStore documentStore;
        private IDocumentSession session;

        public MessageRavenRepository()
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
            var message = session.Load<TEntity>(id);
            return message;
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
                IQueryable<TEntity> tempQuery = session.Query<TEntity>().OrderByDescending(item => item.CreatedDate).Take(128).Skip(start);
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

        public PagedResultDTO<TEntity> GetPaged(PagingInfoDTO pageInfo, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            return null;
        }

        public PagedResultDTO<TEntity> GetPaged(
            PagingInfoDTO pageInfo,
            Expression<Func<TEntity, object>> filterTitleExpression = null,
            string filterTitleField = "",
            Expression<Func<TEntity, object>> filterSubCategoryExpression = null,
            string filterSubCategoryField = "",
            Expression<Func<TEntity, object>> filterTagsExpression = null,
            string filterTagsField = "",
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = null;

            // open a new session on the documentStore defined at the UoW
            session = RavenMessageUoW.documentStore.OpenSession();

            int start = 0;
            while (true)
            {
                // each round of the loop we take 128 regs from the db
                IQueryable<TEntity> tempQuery = session.Query<TEntity>().OrderByDescending(item => item.CreatedDate).Take(128).Skip(start);
                // when there's no more regs to take, we break the loop
                if (tempQuery.ToList().Count() == 0)
                    break;
                start += tempQuery.ToList().Count();

                // chorizo that concats new entries to the query each round of the loop
                query = query == null ? query = tempQuery : query = query.ToList().Concat(tempQuery.ToList()).ToList().AsQueryable();
            }

            // checks if there's no category selected
            if (filterSubCategoryField.Equals("0"))
                filterSubCategoryField = "";

            // first search field: title [use of wildcards for 'contains' kinda like functionality]
            // second search field: category [it's suposed to be exact as the parameter passed]
            query = query
                .Search(filterTitleExpression, "*" + filterTitleField + "*", escapeQueryOptions: EscapeQueryOptions.AllowAllWildcards)
                .Search(filterSubCategoryExpression, "" + filterSubCategoryField, options: SearchOptions.And);

            // filters the results for each one of the tags
            if (filterTagsField != null)
            {
                List<TEntity> queryTags = null;
                // separates each one of the tags entered
                foreach (var tag in filterTagsField.Split(',').ToList<string>())
                {
                    // query to retrieve the results for each specified tag
                    List<TEntity> queryAux = query
                        .Search(filterTagsExpression, "*" + tag + "*", escapeQueryOptions: EscapeQueryOptions.AllowAllWildcards, options: SearchOptions.And).ToList();

                    // intersects the results for each of the tags to see which ones they have in common
                    if (queryTags == null)
                        queryTags = queryAux;
                    else
                        queryTags = queryTags.Intersect(queryAux).ToList();
                }
                // returns the common results
                query = queryTags.AsQueryable();
            }

            IncludeProperties(ref query, includeProperties);
            var orderedQuery = OrderBy(query, orderBy);

			return new PagedResultDTO<TEntity>
			{
				Data = PagedData(orderedQuery, pageInfo),
				PagingInfo = pageInfo
			};

        }

        public void Insert(TEntity entity)
        {
            // open a new session on the documentStore defined at the UoW
            session = RavenMessageUoW.documentStore.OpenSession();
            session.Store(entity);
        }

        public void FileStore(Stream uploadStream, string fileName, RavenJObject metadata)
        {
            // open a new async session
            var asyncSession = RavenMessageUoW.filesStore.OpenAsyncSession();
            // store the files in the db
            using (asyncSession)
            {
                asyncSession.RegisterUpload("files/" + fileName, uploadStream, metadata);
                asyncSession.SaveChangesAsync();
            }
        }

        public async Task<HttpContent> FileRetrieve(string fileId) {
            // open a new async session
            var asyncSession = RavenMessageUoW.filesStore.OpenAsyncSession();
            
            // generate the async stream for retrieving the files
            using (asyncSession)
            {
                var stream = asyncSession.DownloadAsync("files/" + fileId + "_blob");
                stream.Wait();
                var streamContent = new StreamContent(await stream);
                // return the stream
                return streamContent;
            }
        }

        public async Task<RavenJToken> GetFileName(string fileId)
        {
            // open a new async session
            var asyncSession = RavenMessageUoW.filesStore.OpenAsyncSession();

            // generate the async stream for retrieving the files
            using (asyncSession)
            {
                var query = await asyncSession.Query()
                             .WhereEquals("Message", fileId)
                             .ToListAsync();

                RavenJToken realName;
                string realNameString = "";
                if (query.FirstOrDefault().Metadata.TryGetValue("Name", out realName))
                    realNameString = "" + realName;

                return realName;
            }
        }

        public void Delete(int id)
        {
            // open a new session on the documentStore defined at the UoW
            session = RavenMessageUoW.documentStore.OpenSession();

            var message = session.Load<TEntity>(id);
            session.Delete(message);
        }

        public void Update(TEntity entity)
        {
            // open a new session on the documentStore defined at the UoW
            session = RavenMessageUoW.documentStore.OpenSession();
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
