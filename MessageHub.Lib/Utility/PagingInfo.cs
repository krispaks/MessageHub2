﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Utility
{
	public class PagingInfo
	{
		public int Page { get; set; }
		public int Rows { get; set; }
		public int TotalPages { get; set; }
		public int TotalRecords { get; set; }
	}
}
