using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MessageHub.Web.Controllers
{
    public class CommentApiController : ApiController
    {
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
        public void Post([FromBody]string value)
        {
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
