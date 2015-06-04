using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageHub.Web.Models
{
	public class MessageSearchCriteriaViewModel
	{
		public string Title { get; set; }
		public string Tag { get; set; }
		public int Category { get; set; }
		public int SubCategory { get; set; }
		public int Page { get; set; }
		public int Rows { get; set; }
		public int TotalPages { get; set; }
		public int TotalRecords { get; set; }
	}
}