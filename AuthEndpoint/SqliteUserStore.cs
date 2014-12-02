using AuthEndpoint.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using System.Data.SQLite;
using System.Web.Configuration;
using System.IO;

namespace AuthEndpoint
{
    public class SqliteUserStore<TUser> :
        IUserStore<TUser>,
        IUserPasswordStore<TUser>
        where TUser : UserModel
    {
        readonly string _connString;
        public SqliteUserStore(string connectionString)
        {
            _connString = connectionString;
        }

        public async Task CreateAsync(TUser user)
        {
            
            if (user == null) throw new ArgumentNullException("user");
            using (var connection = new SQLiteConnection(_connString))
            {

            }
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            using (var connection = new SQLiteConnection(_connString))
            {

            }
            throw new NotImplementedException();
        }

        public async Task<TUser> FindByIdAsync(string userId)
        {
            using (var connection = new SQLiteConnection(_connString))
            {

            }
            throw new NotImplementedException();
            
        }

        public async Task<TUser> FindByNameAsync(string userName)
        {
            using (var connection = new SQLiteConnection(_connString))
            {

            }
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(TUser user)
        {
            using (var connection = new SQLiteConnection(_connString))
            {

            }
            throw new NotImplementedException();
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