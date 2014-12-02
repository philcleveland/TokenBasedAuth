using AuthEndpoint.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace AuthEndpoint
{
    public class SqliteAuthRepository : IDisposable, IAuthRepository
    {
        readonly UserManager<User> _userMgr;

        public SqliteAuthRepository(string connectionString)
        {
            var userStore = new SqliteUserStore<User>(connectionString);
            _userMgr = new UserManager<User>(userStore);
        }

        public async Task<IdentityResult> RegisterUser(User user, string password)
        {
            var result = await _userMgr.CreateAsync(user, password);
            return result;
        }

        public async Task<User> FindUser(string userName, string password)
        {
            var user = await _userMgr.FindAsync(userName, password);
            return user;
        }

        public async Task<IdentityResult> ChangePassword(string id, string oldPassword, string newPassword)
        {
            var result = await _userMgr.ChangePasswordAsync(id, oldPassword, newPassword);
            return result;
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
                _userMgr.Dispose();
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