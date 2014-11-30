using AuthEndpoint.Models;
using Microsoft.AspNet.Identity;
using Raven.Client;
using System;
using System.Threading.Tasks;

namespace AuthEndpoint
{
    public class RavenDBAuthRepository : IDisposable, IAuthRepository
    {
        IDocumentStore _store;

        public RavenDBAuthRepository(IDocumentStore store)
        {
            _store = store;
        }

        public async Task<IdentityResult> RegisterUser(UserModel user, string password)
        {
            using (var session = _store.OpenAsyncSession())
            {
                var ravenUserStore = new RavenDBUserStore<UserModel>(session);
                var userManager = new UserManager<UserModel>(ravenUserStore);
                var result = await userManager.CreateAsync(user, password);
                await session.SaveChangesAsync();
                return result;
            }
        }

        public async Task<UserModel> FindUser(string userName, string password)
        {
            using (var session = _store.OpenAsyncSession())
            {
                var ravenUserStore = new RavenDBUserStore<UserModel>(session);
                var userManager = new UserManager<UserModel>(ravenUserStore);
                var user = await userManager.FindAsync(userName, password);
                return user;
            }
        }

        public async Task<IdentityResult> ChangePassword(string id, string oldPassword, string newPassword)
        {
            using (var session = _store.OpenAsyncSession())
            {
                var ravenUserStore = new RavenDBUserStore<UserModel>(session);
                var userManager = new UserManager<UserModel>(ravenUserStore);
                var result = await userManager.ChangePasswordAsync(id, oldPassword, newPassword);
                await session.SaveChangesAsync();
                return result;
            }
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