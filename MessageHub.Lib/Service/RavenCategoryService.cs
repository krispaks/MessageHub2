using MessageHub.Lib.DTO;
using MessageHub.Lib.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Service
{
    public class RavenCategoryService : ICategoryService
    {
        private IRavenCategoryUoW _uow = null;
        private ILoggingService _logService = null;

        public RavenCategoryService(IRavenCategoryUoW uow, ILoggingService logService)
        {
            this._uow = uow;
            this._logService = logService;
        }

        public int SaveCategory(Entity.Category category)
        {
            try {
				int retValue = 0;
				this._logService.Log("Start SaveCategory");

				if (Validate(category)) {
					_uow.CategoryRavenRepositoryRepository.Insert(category);
					_uow.SaveChanges();

					retValue = category.Id;
				} else {
					this._logService.Log("Error in Validation");
				}
				return retValue;
			} catch (Exception ex) {
				this._logService.Log(string.Format("Error at SaveCategory : {0}", ex.Message));
				throw;
			} finally {
				this._logService.Log("End SaveCategory");
			}
        }

        private bool Validate(Entity.Category message)
        {
            return true;
        }

        public IEnumerable<Entity.Category> GetCategoryList()
        {
            try {
                this._logService.Log("Start GetCategoryList");

                // gets the list of categories from the db
                var categories = _uow.CategoryRavenRepositoryRepository.Get();
                return categories;
                
            } catch (Exception ex) {
                this._logService.Log(string.Format("Error at GetCategoryList : {0}", ex.Message));
                throw;
            } finally {
                this._logService.Log("End GetCategoryList");
            }
        }

        public Entity.Category GetCategory(int id)
        {
            try {
                this._logService.Log("Start GetCategory");

                // gets a category from the db by its id
                var category = _uow.CategoryRavenRepositoryRepository.Get(id);
                return category;

            } catch (Exception ex) {
                this._logService.Log(string.Format("Error at GetCategory : {0}", ex.Message));
                throw;
            } finally {
                this._logService.Log("End GetCategory");
            }
        }
	}
}
