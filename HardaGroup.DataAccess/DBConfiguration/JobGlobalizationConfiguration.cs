using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess.DBConfiguration
{
    public class JobGlobalizationConfiguration: EntityTypeConfiguration<JobGlobalization>
    {
        internal JobGlobalizationConfiguration()
        {
            this.HasKey(m => m.Id);
            this.Property(c => c.Location).HasMaxLength(200);
            this.Property(c => c.Description).HasMaxLength(200);
            this.Property(a => a.Culture).HasMaxLength(50);
            this.HasRequired(s => s.Job)
                    .WithMany(s => s.JobGlobalizations)
                    .HasForeignKey(s => s.JobId);
        }
    }
}
