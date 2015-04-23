using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Web.Models
{
	interface IViewModel
	{
		int Id { get; set; }
		string CreatedBy { get; set; }
		string CreatedDate { get; set; }
	}
}
