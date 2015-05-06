using MessageHub.Lib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.DTO
{
	public class MessageDetailDTO
	{
		public Message MessageDetail { get; set; }
		public IEnumerable<Comment> CommentList { get; set; }
	}
}
