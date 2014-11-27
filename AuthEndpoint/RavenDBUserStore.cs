using AuthEndpoint.Models;
using Microsoft.AspNet.Identity;
using Raven.Client;
using System;
using System.Threading.Tasks;

namespace AuthEndpoint
{
    public class RavenDBUserStore<TUser> :
        IUserStore<TUser>,
        IUserPasswordStore<TUser>
        where TUser : UserModel
    {
        
        IDocumentStore _docStore;

        public RavenDBUserStore(IDocumentStore docStore)
        {
            if (docStore == null) throw new ArgumentNullException("docStore");
            _docStore = docStore;
        }

        public async Task CreateAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            using (var session = _docStore.OpenAsyncSession())
            {
                await session.StoreAsync(user);
            }
        }

        public async Task DeleteAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            using (var session = _docStore.OpenAsyncSession())
            {
                session.Delete(user);
                await Task.FromResult<object>(null);
            }
        }

        public async Task<TUser> FindByIdAsync(string userId)
        {
            using (var session = _docStore.OpenAsyncSession())
            {
                return await session.Query<TUser>()
                    .FirstOrDefaultAsync(u => u.Id == userId);
            }
        }

        public async Task<TUser> FindByNameAsync(string userName)
        {
            using (var session = _docStore.OpenAsyncSession())
            {
                return await session.Query<TUser>()
                    .FirstOrDefaultAsync<TUser>(u => u.UserName == userName);
            }
        }

        public async Task UpdateAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            using (var session = _docStore.OpenAsyncSession())
            {
                await session.StoreAsync(user);
            }
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult<bool>(!string.IsNullOrEmpty(user.Password));
        }

        public System.Threading.Tasks.Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}