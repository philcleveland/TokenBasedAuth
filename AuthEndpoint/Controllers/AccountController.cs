using AuthEndpoint.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web.Http;


namespace AuthEndpoint.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        readonly IAuthRepository _authRepo;
        readonly AppUserManager _userMgr;
        public AccountController(IAuthRepository authRepo, AppUserManager userMgr)
        {
            if (authRepo == null) throw new ArgumentNullException("authRepo");
            if (userMgr == null) throw new ArgumentNullException("userMgr");
            _authRepo = authRepo;
            _userMgr = userMgr;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(UserViewModel user)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userModel = new User()
            {
                UserName = user.UserName,
                Email = user.Email,
            };

            //TODO: make up a password here and send it on the confirmation email?
            var result = await _authRepo.RegisterUser(userModel, user.Password);
            var errorResult = GetErrorResult(result);
            if (errorResult != null) return errorResult;

            if (result.Succeeded)
            {
                var code = _userMgr.GenerateEmailConfirmationToken(userModel.Id);
                var callbackUrl = Url.Link("DefaultApi", new { Controller = "Account", Action = "ConfirmEmail", UserId = userModel.Id, Code = code });
                await _userMgr.SendEmailAsync(userModel.Id, "Success", callbackUrl);
            }

            return Ok();
        }

        [Route("changepassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var result = await _authRepo.ChangePassword(model.UserID, model.OldPassword, model.NewPassword);
            return Ok();
        }

        [Route("confirmemail")]
        [HttpGet]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        {
            var result = await _userMgr.ConfirmEmailAsync(userId, code);
            var errorResult = GetErrorResult(result);
            if (errorResult != null) return errorResult;
            return Ok();
        }

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
