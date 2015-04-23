using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageHub.Web.Models
{
	public class MessageListViewModel : IViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string ContentConcat { get; set; }
		public string Category { get; set; }
		public string CreatedBy { get; set; }
		public string CreatedDate { get; set; }
	}
}