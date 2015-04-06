using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageHub.Web.Models
{
	public class MessageSearchCriteria
	{
		public string Title { get; set; }
		public string Tag { get; set; }
		public int Category { get; set; }
		public int SubCategory { get; set; }
	}
}