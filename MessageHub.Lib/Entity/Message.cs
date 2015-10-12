using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Entity
{
	public partial class Message : BaseEntity
	{
		public Message() 
		{
			this.Comments = new HashSet<Comment>();
		}

		[Required]
		[StringLength(250)]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

        public string Tags { get; set; }
		public int SubCategoryId { get; set; }
		public virtual Category Category { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
	}
}
