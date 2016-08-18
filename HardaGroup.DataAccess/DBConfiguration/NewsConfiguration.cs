using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess.DBConfiguration
{
    public class NewsConfiguration : EntityTypeConfiguration<News>
    {
        internal NewsConfiguration()
        {
            this.HasKey(m => m.Id);
            this.Property(c => c.Description).HasMaxLength(200);
            this.HasRequired<NewsScope>(s => s.NewsScope)
                    .WithMany(s => s.News)
                    .HasForeignKey(s => s.NewsScopeId);

        }
    }
}
