using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageHub.Web.Models
{
	public class MessageViewModel : IViewModel
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string CreatedBy { get; set; }
		public string CreatedDate { get; set; }
	}
}