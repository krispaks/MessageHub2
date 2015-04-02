using MessageHub.Lib.Entity;
using MessageHub.Lib.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MessageHub.Web.Controllers
{
    public class MessageController : ApiController
    {
	    private IMessageUoW messageUoW = null;

	    public MessageController(IMessageUoW messageUoW)
	    {
			this.messageUoW = messageUoW;
	    }

	    // GET: api/Message
        public IEnumerable<Message> Get()
        {
	        return this.messageUoW.MessageHubRepositoryRepository.Get();
        }

        // GET: api/Message/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Message
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Message/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Message/5
        public void Delete(int id)
        {
        }
    }
}
