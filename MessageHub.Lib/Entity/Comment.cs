using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Entity
{
	public partial class Comment : BaseEntity
	{	
		public int MessageId { get; set; }

		[Required]
		public string Value { get; set; }

		public virtual Message Message { get; set; }
	}
}
