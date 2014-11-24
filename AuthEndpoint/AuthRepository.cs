using AuthEndpoint.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AuthEndpoint
{
    public class AuthRepository : IDisposable, AuthEndpoint.IAuthRepository
    {
        readonly AuthContext _ctx;
        readonly UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(UserModel user)
        {
            var identUser = new IdentityUser
            {
                UserName = user.UserName,
            };
            var result = await _userManager.CreateAsync(identUser, user.Password);
            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // get rid of managed resources
                _ctx.Dispose();
                _userManager.Dispose();
            }
            // get rid of unmanaged resources
        }

        // only if you use unmanaged resources directly in AuthRepository
        //AuthRepository()
        //{
        //    Dispose(false);
        //}
    }
}