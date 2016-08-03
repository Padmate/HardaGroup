namespace HardaGroup.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<HardaGroup.DataAccess.HardaDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(HardaGroup.DataAccess.HardaDBContext context)
        {
            #region Add Admin Role
            string adminRoleId = string.Empty;
            string admin = "Admin";
            var adminRole = context.Roles.FirstOrDefault(r => r.Name == admin);
            if (adminRole == null)
            {
                var adminModel = new IdentityRole
                {
                    Name = admin
                };
                context.Roles.Add(adminModel);
                adminRoleId = adminModel.Id;
            }
            else
            {
                adminRoleId = adminRole.Id;
            }

            #endregion
            #region Init Admin User
            string userName = "Admin";
            string password = "admin123";
            var user = context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                // Hash password
                var passwordHash = new PasswordHasher().HashPassword(password);

                var userModel = new ApplicationUser
                {
                    UserName = userName,
                    PasswordHash = passwordHash,
                    Email = "123@qq.com",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                context.Users.Add(userModel);
                //Add User to Admin
                var relation = new IdentityUserRole() { UserId = userModel.Id, RoleId = adminRoleId };
                userModel.Roles.Add(relation);
            }
            #endregion
            context.SaveChanges();
        }
    }
}
