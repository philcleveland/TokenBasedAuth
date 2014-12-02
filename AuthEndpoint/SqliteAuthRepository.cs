using AuthEndpoint.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace AuthEndpoint
{
    public class SqliteAuthRepository : IDisposable, IAuthRepository
    {

        readonly string _connStr;
        public SqliteAuthRepository(string connectionString)
        {
            _connStr = connectionString;
        }

        public async Task<IdentityResult> RegisterUser(UserModel user, string password)
        {
            var userStore = new SqliteUserStore<UserModel>(_connStr);
            var userManager = new UserManager<UserModel>(userStore);
            var result = await userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<UserModel> FindUser(string userName, string password)
        {
            var userStore = new SqliteUserStore<UserModel>(_connStr);
            var userManager = new UserManager<UserModel>(userStore);
            var user = await userManager.FindAsync(userName, password);
            return user;
        }

        public async Task<IdentityResult> ChangePassword(string id, string oldPassword, string newPassword)
        {
            var userStore = new SqliteUserStore<UserModel>(_connStr);
            var userManager = new UserManager<UserModel>(userStore);
            var result = await userManager.ChangePasswordAsync(id, oldPassword, newPassword);
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
                //_ctx.Dispose();
                //_userManager.Dispose();
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