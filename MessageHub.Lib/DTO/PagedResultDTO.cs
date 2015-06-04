using MessageHub.Lib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.DTO
{
	public class PagedResultDTO<TEntity>
		where TEntity : BaseEntity
	{
		public IQueryable<TEntity> Data { get; set; }
		public PagingInfoDTO PagingInfo { get; set; }
	}
}
