using AuthEndpoint.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;

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

        public async Task<IdentityResult> RegisterUser(string name, string password)
        {
            var identUser = new IdentityUser
            {
                UserName = name,
            };
            var result = await _userManager.CreateAsync(identUser, password);
            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public async Task<IdentityResult> ChangePassword(string id, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(id, oldPassword, newPassword);
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