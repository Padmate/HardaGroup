using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }

    public class User
    {
        public int AccessFailedCount { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Id { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }

        public string UserType { get; set; }
        public ICollection<Role> Roles { get; set; }

        public ApplicationUser ConverToApplicationUser(User user)
        {
            ApplicationUser result = new ApplicationUser();
            if (user != null)
            {
                result.Id = user.Id;
                result.UserName = user.UserName;
                result.PasswordHash = user.PasswordHash;
                result.Email = user.Email;
                result.EmailConfirmed = user.EmailConfirmed;
                result.PhoneNumber = user.PhoneNumber;
                result.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                result.SecurityStamp = user.SecurityStamp;
                result.TwoFactorEnabled = user.TwoFactorEnabled;
                result.AccessFailedCount = user.AccessFailedCount;
                result.LockoutEnabled = user.LockoutEnabled;
                result.LockoutEndDateUtc = user.LockoutEndDateUtc;


            }
            return result;
        }
    }
}
