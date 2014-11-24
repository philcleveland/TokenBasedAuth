using AuthEndpoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuthEndpoint.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        readonly AuthRepository _authRepo;
        
        public AccountController()
        {
            _authRepo = new AuthRepository();
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(UserModel user)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _authRepo.RegisterUser(user);
            var errorResult = GetErrorResult(result);
            if (errorResult != null) return errorResult;
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // get rid of managed resources
                _authRepo.Dispose();
            }
            // get rid of unmanaged resources
            
        }

        // only if you use unmanaged resources directly in AccountController
        //AccountController()
        //{
        //    Dispose(false);
        //}

        private IHttpActionResult GetErrorResult(Microsoft.AspNet.Identity.IdentityResult result)
        {
            if (result == null) return InternalServerError();
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return BadRequest(ModelState);
            }
            return null;
        }
    }
}
