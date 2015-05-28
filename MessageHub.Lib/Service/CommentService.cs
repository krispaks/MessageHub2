using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.Repository;
using MessageHub.Lib.Entity;
using MessageHub.Lib.Utility;

namespace MessageHub.Lib.Service
{
	public class CommentService : ICommentService
	{
		private ILoggingService _loggingService = null;
		private IRepository<Comment, MessageHubDbContext> _commentRepository = null;

		public CommentService(IRepository<Comment, MessageHubDbContext> commentRepository, ILoggingService loggingService)
		{
			this._commentRepository = commentRepository;
			this._loggingService = loggingService;
		}

		public int SaveComment(Comment comment)
		{
			try
			{
				_loggingService.Log("Start SaveComment");
				int retValue = 0;

				if (Validate(comment))
				{
					comment.CreatedDate = UtilityDate.HubDateTime();
					comment.CreatedBy = 1;
					_commentRepository.Insert(comment);
					retValue = _commentRepository.Save();
				}
				else
				{
					this._loggingService.Log("Error in Validation");
				}

				return retValue;
			}
			catch (Exception ex)
			{
				this._loggingService.Log(string.Format("Error at SaveComment : {0}", ex.Message));
				throw;
			}
			finally
			{
				this._loggingService.Log("End SaveComment");
			}
		}

        public IEnumerable<Entity.Comment> GetComments(int id)
        {
            return null;
        }

		private bool Validate(Comment comment)
		{
			return true;
		}
	}
}
