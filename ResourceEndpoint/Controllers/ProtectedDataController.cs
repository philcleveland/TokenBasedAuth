using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace ResourceEndpoint.Controllers
{
    [Authorize]
    [RoutePrefix("api/protected")]
    public class ProtectedDataController : ApiController
    {
        [Route("data")]
        [HttpGet]
        public IEnumerable<object> Get()
        {
            var identity = User.Identity as ClaimsIdentity;
            var claims = identity.Claims.Select(x => new { Type = x.Type, Value = x.Value, Issuer = x.Issuer, OrigIssuer = x.OriginalIssuer });
            return claims;
        }
    }
}
