using AuthEndpoint.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;

namespace AuthEndpoint
{
    public class AppUserManager : UserManager<User>
    {
        public AppUserManager(IUserStore<User> store, 
            IdentityFactoryOptions<AppUserManager> options,
            IIdentityMessageService emailSvc)
            : base(store)
        {
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;
            this.UserTokenProvider = new DataProtectorTokenProvider<User>(options.DataProtectionProvider.Create());
            this.EmailService = emailSvc;
        }

        
    }
}
