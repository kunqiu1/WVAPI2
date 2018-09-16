using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace WVAPI2.Controllers
{
    [RoutePrefix("api/values")]
    //[System.Web.Http.Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        [HttpGet]
        [ActionName("wocao")]
        public string Get()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;

            return username;
        }

        // GET api/values/5
        public string Get(int id)
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            return username+" niubi";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
