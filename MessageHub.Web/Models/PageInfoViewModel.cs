using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageHub.Web.Models
{
	public class PageInfoViewModel
	{
		public int Page { get; set; }
		public int Rows { get; set; }
		public int TotalPages { get; set; }
		public int TotalRecords { get; set; }
	}
}