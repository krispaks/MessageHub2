using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Utility
{
	public class UtilityDate
	{
		public static DateTime HubDateTime()
		{
			return DateTime.Now;
		}

		public static string HubDateString(DateTime dateTime)
		{
			//return dateTime.ToShortDateString();
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff");
		}
	}
}
