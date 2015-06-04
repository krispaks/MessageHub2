using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageHub.Web.Models
{
	public class MessageDetailViewModel
	{
		public MessageViewModel Message { get; set; }
		public CommentViewModel NewComment { get; set; }
		public List<CommentViewModel> CommentList { get; set; }
	}
}