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
        IAsyncDocumentSession Session { get; set; }
        public RavenDBUserStore(IAsyncDocumentSession session)
        {
            Session = session;
        }

        public async Task CreateAsync(TUser user)
        {
            
            if (user == null) throw new ArgumentNullException("user");
            await Session.StoreAsync(user);
        }

        public async Task DeleteAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            Session.Delete(user);
            await Task.FromResult<object>(null);
        }

        public async Task<TUser> FindByIdAsync(string userId)
        {
            return await Session.Query<TUser>()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<TUser> FindByNameAsync(string userName)
        {
            return await Session.Query<TUser>()
                .FirstOrDefaultAsync<TUser>(u => u.UserName == userName);
        }

        public async Task UpdateAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            await Session.StoreAsync(user);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult<bool>(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (user == null) throw new ArgumentNullException("user");
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public void Dispose()
        {

        }
    }
}