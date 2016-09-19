using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess.DBConfiguration
{
    public class JobConfiguration: EntityTypeConfiguration<Job>
    {
        internal JobConfiguration()
        {
            this.HasKey(m => m.Id);
            this.Property(c => c.URLId).HasMaxLength(200);
            this.HasRequired<JobScope>(s => s.JobScope)
                    .WithMany(s => s.Jobs)
                    .HasForeignKey(s => s.JobScopeId);

        }
    }
}
