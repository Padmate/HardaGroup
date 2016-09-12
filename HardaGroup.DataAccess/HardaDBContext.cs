﻿using HardaGroup.DataAccess.DBConfiguration;
using HardaGroup.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }

    public class HardaDBContext : IdentityDbContext<ApplicationUser>
    {
        public HardaDBContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<AboutGlobalization> AboutGlobalizations { get; set; }
        public DbSet<NewsScope> NewsScopes { get; set; }
        public DbSet<NewsScopeGlobalization> NewsScopeGlobalizations { get; set; }

        public DbSet<News> News { get; set; }
        public DbSet<NewsGlobalization> NewsGlobalizations { get; set; }

        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleGlobalization> ModuleGlobalizations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new ImageConfiguration());
            modelBuilder.Configurations.Add(new AboutConfiguration());
            modelBuilder.Configurations.Add(new AboutGlobalizationConfiguration());
            modelBuilder.Configurations.Add(new NewsScopeConfiguration());
            modelBuilder.Configurations.Add(new NewsScopeGlobalizationConfiguration());
            modelBuilder.Configurations.Add(new NewsConfiguration());
            modelBuilder.Configurations.Add(new NewsGlobalizationConfiguration());
            modelBuilder.Configurations.Add(new ModuleConfiguration());
            modelBuilder.Configurations.Add(new ModuleGlobalizationConfiguration());

        }

        public static HardaDBContext Create()
        {
            return new HardaDBContext();
        }
    }
}
