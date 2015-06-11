using MessageHub.Lib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.DTO;

namespace MessageHub.Lib.Service
{
	public interface ICategoryService
	{
		int SaveCategory(Category category);
		IEnumerable<Category> GetCategoryList();
		Category GetCategory(int id);
	}
}
