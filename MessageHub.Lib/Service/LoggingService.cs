using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Service
{
	public class LoggingService : ILoggingService
	{
		public bool Log(string logMessage)
		{
			Console.WriteLine(logMessage);
			return true;
		}
	}
}
