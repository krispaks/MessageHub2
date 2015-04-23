using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Raven.Client.Document;
using Raven.Client.FileSystem;
using Raven.Json.Linq;
//using Raven.Database.Server;
using Raven.Client;

namespace MessageHub.Web.Controllers
{
    public class RavenMessageController : ApiController
    {
        private DocumentStore documentStore;

        // DB initialize
        public void StartDB() {            
            documentStore = new DocumentStore {
                Url = "http://localhost:8080/",
                DefaultDatabase = "MessageHubDB"
            };
            documentStore.Initialize();
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}