using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Service
{
	public interface ILoggingService
	{
		bool Log(string logMessage);
	}
}
