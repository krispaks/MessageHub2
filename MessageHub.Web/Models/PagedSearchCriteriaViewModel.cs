using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageHub.Web.Models
{
	public class PagedSearchCriteriaViewModel<TSearchCriteria>
		where TSearchCriteria : class
	{
		public TSearchCriteria SearchCriteria { get; set; }
		public PageInfoViewModel PageInfo { get; set; }
	}

	public class PagedSearchCriteriaViewModel
	{
		public MessageSearchCriteriaViewModel SearchCriteria { get; set; }
		public PageInfoViewModel PageInfo { get; set; }
	}
}