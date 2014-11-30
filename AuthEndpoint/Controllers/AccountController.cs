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
        readonly IAuthRepository _authRepo;
        public AccountController(IAuthRepository authRepo)
        {
            //_authRepo = new AuthRepository();
            if (authRepo == null) throw new ArgumentNullException("userStore");
            _authRepo = authRepo;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(UserViewModel user)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userModel = new UserModel()
            {
                UserName = user.UserName,
                Email = user.Email,
            };

            var result = await _authRepo.RegisterUser(userModel, user.Password);
            var errorResult = GetErrorResult(result);
            if (errorResult != null) return errorResult;
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
            if (!result.Succeeded)
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
