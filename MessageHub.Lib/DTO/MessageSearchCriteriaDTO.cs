using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.Utility;

namespace MessageHub.Lib.DTO
{
	public class MessageSearchCriteriaDTO
	{
		public string Title { get; set; }
		public string Tag { get; set; }
		public int SubCategory { get; set; }
		public PagingInfo PagingInfo { get; set; }
	}
}
