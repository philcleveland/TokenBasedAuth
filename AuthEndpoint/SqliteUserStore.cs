using AuthEndpoint.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNet.Identity;
using System;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace AuthEndpoint
{
    public class SqliteUserStore<TUser> :
        IUserStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserEmailStore<TUser>
        where TUser : User
    {
        public static readonly string GetByNameStatement = @"SELECT * FROM users WHERE UserName = @userName";
        public static readonly string GetByEmailStatement = @"SELECT * FROM users WHERE Email = @email";
        public static readonly string InsertUserStatement = @"insert into Users ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName])
                                                                          Values(@Id, @Email, @EmailConfirmed, @PasswordHash, @SecurityStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEndDateUtc, @LockoutEnabled, @AccessFailedCount, @UserName)";
        readonly string _connString;
        public SqliteUserStore(string connectionString)
        {
            _connString = connectionString;
        }

        public async Task CreateAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            using (var cn = GetOpenConnection())
            {
                await cn.ExecuteAsync(InsertUserStatement, user);
            }
        }

        public async Task DeleteAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            using (var cn = GetOpenConnection())
            {
                await cn.DeleteAsync<TUser>(user);
            }
        }

        public async Task<TUser> FindByIdAsync(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            using (var cn = GetOpenConnection())
            {
                var user = await cn.GetAsync<TUser>(userId);
                return user;
            }
        }

        public async Task<TUser> FindByNameAsync(string userName)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            using (var cn = GetOpenConnection())
            {
                var user = await cn.QueryAsync<TUser>(GetByNameStatement, new { UserName = userName });
                return user.FirstOrDefault();
            }
        }

        public async Task UpdateAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            using (var cn = GetOpenConnection())
            {
                await cn.UpdateAsync<TUser>(user);
            }
        }

        public async Task<TUser> FindByEmailAsync(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            using (var cn = GetOpenConnection())
            {
                var user = await cn.QueryAsync<TUser>(GetByEmailStatement, new { Email = email });
                return user.FirstOrDefault();
            }
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return Task.FromResult(user.EmailConfirmed == 1 ? true : false);
        }

        public async Task SetEmailAsync(TUser user, string email)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (email == null) throw new ArgumentNullException("email");
            using (var cn = GetOpenConnection())
            {
                user.Email = email;
                await cn.UpdateAsync<TUser>(user);
            }
        }

        public async Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null) throw new ArgumentNullException("user");
            using (var cn = GetOpenConnection())
            {
                user.EmailConfirmed = confirmed == true ? 1 : 0;
                await cn.UpdateAsync<TUser>(user);
            }
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

        private SQLiteConnection GetOpenConnection()
        {
            var connection = new SQLiteConnection(_connString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Use this to attach to connection.Trace event if you want the SQL being executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connection_Trace(object sender, TraceEventArgs e)
        {
            var curColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.WriteLine(e.Statement);
            Console.BackgroundColor = curColor;
        }


    }
}