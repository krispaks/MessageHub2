using MessageHub.Lib.Entity;
using MessageHub.Lib.Service;
using MessageHub.Lib.Utility;
using MessageHub.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MessageHub.Web.Controllers
{
	[Authorize]
    public class CategoryApiController : ApiController
    {
        private ICategoryService categoryService = null;
		private ILoggingService logger = null;

		public CategoryApiController(ICategoryService categoryService, ILoggingService logger)
		{
			this.categoryService = categoryService;
			this.logger = logger;
		}

        // GET: api/CategoryApi
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var pagedResult = this.categoryService.GetCategoryList();

                foreach(var item in pagedResult){
                    int itemId = item.Id;
                }

                List<CategoryViewModel> categoryList = pagedResult.Select(category => new CategoryViewModel
                {
                    Id = category.Id,
                    ParentId = (long)category.ParentId,
                    //ParentId = 0,
                    Name = category.Name,
                    Description = category.Description,
                    CreatedBy = "KPACA",
                    CreatedDate = UtilityDate.HubDateString(category.CreatedDate)
                }).ToList();

                response = Request.CreateResponse(HttpStatusCode.OK, categoryList);
            }
            catch (Exception ex)
            {
                this.logger.Log(string.Format("Error at Category Get : {0}", ex.Message));
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }

        // GET: api/CategoryApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CategoryApi
        public HttpResponseMessage Post(CategoryViewModel newCategory)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                Category category = new Category
                {
                    /*Name = newCategory.Name,
                    Description = newCategory.Description,
                    CreatedBy = 1,
                    CreatedDate = UtilityDate.HubDateTime()*/

                    Name = "Category 1 - A",
                    Description = "Subcategory A for Category 1",
                    CreatedBy = 1,
                    CreatedDate = UtilityDate.HubDateTime()
                };

                int value = this.categoryService.SaveCategory(category);

                response = Request.CreateResponse(HttpStatusCode.OK, value);
            }
            catch (Exception ex)
            {
                this.logger.Log(string.Format("Error at Message Get : {0}", ex.Message));
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }

        // PUT: api/CategoryApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CategoryApi/5
        public void Delete(int id)
        {
        }
    }
}
