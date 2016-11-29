using HardaGroup.DataAccess.DBConfiguration;
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

        public DbSet<JobScope> JobScopes { get; set; }
        public DbSet<JobScopeGlobalization> JobScopeGlobalizations { get; set; }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobGlobalization> JobGlobalizations { get; set; }


        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleGlobalization> ModuleGlobalizations { get; set; }

        public DbSet<Mail> Mails { get; set; }
        public DbSet<MailAttachment> MailAttachements { get; set; }

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
            modelBuilder.Configurations.Add(new JobScopeConfiguration());
            modelBuilder.Configurations.Add(new JobScopeGlobalizationConfiguration());
            modelBuilder.Configurations.Add(new JobConfiguration());
            modelBuilder.Configurations.Add(new JobGlobalizationConfiguration());
            modelBuilder.Configurations.Add(new ModuleConfiguration());
            modelBuilder.Configurations.Add(new ModuleGlobalizationConfiguration());
            modelBuilder.Configurations.Add(new MailConfiguration());
            modelBuilder.Configurations.Add(new MailAttachmentConfiguration());

        }

        public static HardaDBContext Create()
        {
            return new HardaDBContext();
        }
    }
}
