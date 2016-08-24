using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess.DBConfiguration
{
    public class NewsGlobalizationConfiguration : EntityTypeConfiguration<NewsGlobalization>
    {
        internal NewsGlobalizationConfiguration()
        {
            this.HasKey(m => m.Id);
            this.Property(c => c.Description).HasMaxLength(200);
            this.Property(a => a.Culture).HasMaxLength(50);
            this.HasRequired(s => s.News)
                    .WithMany(s => s.NewsGlobalizations)
                    .HasForeignKey(s => s.NewsId);
        }
    }
}
