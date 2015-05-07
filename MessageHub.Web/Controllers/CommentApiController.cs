﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MessageHub.Lib.Service;
using MessageHub.Web.Models;
using MessageHub.Lib.Entity;

namespace MessageHub.Web.Controllers
{
    public class CommentApiController : ApiController
    {
	    private ICommentService _commentService = null;
		private ILoggingService _loggingService = null;

	    public CommentApiController(ICommentService commentService, ILoggingService loggingService)
	    {
		    this._commentService = commentService;
		    this._loggingService = loggingService;
	    }

        // GET: api/CommentApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CommentApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CommentApi
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

        // PUT: api/CommentApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CommentApi/5
        public void Delete(int id)
        {
        }
    }
}
