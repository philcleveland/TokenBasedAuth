using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AuthEndpoint.Models
{
    

    public class User : IUser
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString("N");
            this.UserName = "";
            this.PasswordHash = "";
            this.SecurityStamp = "";
            this.PhoneNumber = "";
            this.PhoneNumberConfirmed = 0;
            this.TwoFactorEnabled = 0;
            this.Email = "";
            this.EmailConfirmed = 0;
            this.LockoutEnabled = 0;
            this.AccessFailedCount = 0;
        }
        
        public string Id { get; set; }
        public string Email { get; set; }
        public int EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneNumberConfirmed { get; set; }
        public int TwoFactorEnabled { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }
        public int LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
    }
}