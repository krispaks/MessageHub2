using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageHub.Web.Models
{
	public class PagedResultViewModel<TItemViewModel>
		where TItemViewModel : class 
	{
		public IEnumerable<TItemViewModel> Data { get; set; }
		public PageInfoViewModel PageInfo { get; set; }
	}
}