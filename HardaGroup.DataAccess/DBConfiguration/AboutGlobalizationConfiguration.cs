using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess.DBConfiguration
{
    public class AboutGlobalizationConfiguration : EntityTypeConfiguration<AboutGlobalization>
    {
        internal AboutGlobalizationConfiguration()
        {
            this.HasKey(a => a.Id);
            this.Property(a => a.TypeName).HasMaxLength(200);
            this.Property(a => a.Culture).HasMaxLength(50);

            this.HasRequired(a => a.About)
                .WithMany(a => a.AboutGlobalizations)
                .HasForeignKey(a => a.AboutId);
        }
    }

}
