using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HookahCRM.Controllers
{
    [Authorize]
    public class SecureDataController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok(new { secureData = "You have to be authenticated to access this!" });
        }
    }
}