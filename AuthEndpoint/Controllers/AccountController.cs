using AuthEndpoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
namespace AuthEndpoint.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        //readonly AuthRepository _authRepo;
        readonly RavenDBUserStore<UserModel> _userStore;
        public AccountController(RavenDBUserStore<UserModel> userStore)
        {
            //_authRepo = new AuthRepository();
            if (userStore == null) throw new ArgumentNullException("userStore");
            _userStore = userStore;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(UserModel user)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _userStore.CreateAsync(user);
            //var errorResult = GetErrorResult(result);
            //if (errorResult != null) return errorResult;
            return Ok();
        }

        //[Route("changepassword")]
        //[HttpPost]
        //public async Task<IHttpActionResult> ChangePassword(ChangePasswordModel model)
        //{
        //    var result = await _authRepo.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
        //    return Ok();
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // get rid of managed resources
                //_authRepo.Dispose();
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
