using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess.DBConfiguration
{
    public class ModuleGlobalizationConfiguration : EntityTypeConfiguration<ModuleGlobalization>
    {
        internal ModuleGlobalizationConfiguration()
        {
            this.HasKey(m => m.Id);
            this.Property(c => c.Description).HasMaxLength(200);
            this.Property(a => a.Culture).HasMaxLength(50);

            this.HasRequired(s => s.Module)
                    .WithMany(s => s.ModuleGlobalizations)
                    .HasForeignKey(s => s.ModuleId);
        }
    }
}
