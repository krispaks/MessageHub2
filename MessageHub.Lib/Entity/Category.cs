using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Entity
{
	public partial class Category : BaseEntity
	{
		public Category()
		{
			this.Categories1 = new HashSet<Category>();
			this.Messages = new HashSet<Message>();
		}

		public int? ParentId { get; set; }
		[Required]
		[StringLength(250)]
		public string Name { get; set; }

		[StringLength(500)]
		public string Description { get; set; }		

		public virtual ICollection<Category> Categories1 { get; set; }
		public virtual Category Category1 { get; set; }
		public virtual ICollection<Message> Messages { get; set; }
	}
}
