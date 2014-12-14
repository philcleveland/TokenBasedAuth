using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AuthEndpoint.Models
{


    public class User : IdentityUser, IUser
    {
        public User():base()
        {
            this.Id = Guid.NewGuid().ToString("N");
            this.UserName = "";
            this.PasswordHash = "";
            this.SecurityStamp = "";
            this.PhoneNumber = "";
            this.PhoneNumberConfirmed = false;
            this.TwoFactorEnabled = false;
            this.Email = "";
            this.EmailConfirmed = false;
            this.LockoutEnabled = false;
            this.AccessFailedCount = 0;
        }
        
    }
}