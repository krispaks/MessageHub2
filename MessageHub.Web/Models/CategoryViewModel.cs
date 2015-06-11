using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageHub.Web.Models
{
	public class CategoryViewModel : IViewModel
	{
		public long Id { get; set; }
        public long ParentId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string CreatedBy { get; set; }
		public string CreatedDate { get; set; }
	}
}