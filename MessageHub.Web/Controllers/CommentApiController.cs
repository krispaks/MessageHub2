using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MessageHub.Lib.Service;
using MessageHub.Web.Models;
using MessageHub.Lib.Entity;
using MessageHub.Lib.Utility;

namespace MessageHub.Web.Controllers
{
	[Authorize]
    public class CommentApiController : ApiController
    {
	    private ICommentService _commentService = null;
		private ILoggingService _loggingService = null;

	    public CommentApiController(ICommentService commentService, ILoggingService loggingService)
	    {
		    this._commentService = commentService;
		    this._loggingService = loggingService;
	    }
		
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // returns the list of comments posted for a specified message id
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var commentList = new List<CommentViewModel>();

            //var message = this.messageService.GetMessage(id);
            var comments = this._commentService.GetComments(id);

            foreach (var comm in comments)
            {
                commentList.Add(new CommentViewModel
                {
                    Id = comm.Id,
                    MessageId = comm.MessageId,
                    Value = comm.Value,
                    CreatedBy = "KPACA",
                    CreatedDate = UtilityDate.HubDateString(comm.CreatedDate)

                });
            }

            /*var vm = new MessageViewModel();

			vm.CommentList = new List<CommentViewModel>();

            var comments = this._commentService.GetComments(id);
            foreach (var comm in comments)
            {
                vm.CommentList.Add(new CommentViewModel
                {
                    Id = comm.Id,
                    MessageId = comm.MessageId,
                    Value = comm.Value,
                    CreatedBy = "KPACA",
                    CreatedDate = UtilityDate.HubDateString(comm.CreatedDate)

                });
            }*/

            response = Request.CreateResponse(HttpStatusCode.OK, commentList);

            return response;
        }

		public HttpResponseMessage Post(CommentViewModel newComment)
        {
			HttpResponseMessage response = new HttpResponseMessage();

	        try
	        {
		        var comment = new Comment
		        {
			        Value = newComment.Value,
					MessageId = newComment.MessageId
		        };

				int value = _commentService.SaveComment(comment);

		        response = Request.CreateResponse(HttpStatusCode.OK, value);
	        }
	        catch (Exception ex)
	        {
				this._loggingService.Log(string.Format("Error at Comment Post : {0}", ex.Message));
				response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
	        }

	        return response;
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}
