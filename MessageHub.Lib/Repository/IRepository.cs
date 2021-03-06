﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.DTO;
using MessageHub.Lib.Entity;
using System.IO;
using Raven.Json.Linq;
using System.Net.Http;

namespace MessageHub.Lib.Repository
{
	public interface IRepository<TEntity, TContext> : IDisposable
		where TEntity : BaseEntity
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
		IEnumerable<TEntity> Get(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = "");

		/// <summary>
		/// Gets a paged entity based on filter and ordering by property
		/// </summary>
		PagedResultDTO<TEntity> GetPaged(PagingInfoDTO pageInfo,
			Expression<Func<TEntity, bool>> filter = null,
            //System.Linq.Expressions.Expression<System.Func<TEntity, object>> filterExpression = null,
            //string filterField = "",
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = "");

        /// <summary>
        /// Gets a paged entity based on filter and ordering by property
        /// </summary>
        PagedResultDTO<TEntity> GetPaged(
            PagingInfoDTO pageInfo,
            Expression<Func<TEntity, object>> filterTitleExpression = null,
            string filterTitleField = "",
            Expression<Func<TEntity, object>> filterSubCategoryExpression = null,
            string filterSubCategoryField = "",
            Expression<Func<TEntity, object>> filterTagsExpression = null,
            string filterTagsField = "",
            //System.Linq.Expressions.Expression<System.Func<TEntity, object>> filterExpression = null,
            //string filterField = "",
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

		/// <summary>
		/// Save Data in DataStore
		/// </summary>
		void Insert(TEntity entity);

        /// <summary>
        /// Save Files in FilesStore
        /// </summary>
        void FileStore(Stream uploadStream, string fileName, RavenJObject metadata);

        /// <summary>
        /// retrieve Files from FilesStore
        /// </summary>
        Task<HttpContent> FileRetrieve(string fileId);

        /// <summary>
        /// retrieve a file's name from FilesStore
        /// </summary>
        Task<RavenJToken> GetFileName(string fileId);

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
