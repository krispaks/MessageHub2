using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageHub.Web.Models
{
	public class CommentViewModel : IViewModel
	{
		public long Id { get; set; }
		public int MessageId { get; set; }
		public string Value { get; set; }
		public string CreatedBy { get; set; }
		public string CreatedDate { get; set; }
	}
}