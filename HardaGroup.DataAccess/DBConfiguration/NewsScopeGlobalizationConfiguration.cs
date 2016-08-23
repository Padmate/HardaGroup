using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess.DBConfiguration
{
    public class NewsScopeGlobalizationConfiguration:EntityTypeConfiguration<NewsScopeGlobalization>
    {
        internal NewsScopeGlobalizationConfiguration()
        {
            this.HasKey(ns=>ns.Id);
            this.Property(ns => ns.TypeName).HasMaxLength(200);
            this.Property(ns => ns.Culture).HasMaxLength(50);

            this.HasRequired(ns => ns.NewsScope)
                .WithMany(ns => ns.NewsScopeGlobalizations)
                .HasForeignKey(ns=>ns.NewsScopeId);
        }
    }
}
