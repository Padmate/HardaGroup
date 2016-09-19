using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess.DBConfiguration
{
    public class JobScopeGlobalizationConfiguration:EntityTypeConfiguration<JobScopeGlobalization>
    {
        internal JobScopeGlobalizationConfiguration()
        {
            this.HasKey(ns=>ns.Id);
            this.Property(ns => ns.TypeName).HasMaxLength(200);
            this.Property(ns => ns.Culture).HasMaxLength(50);

            this.HasRequired(ns => ns.JobScope)
                .WithMany(ns => ns.JobScopeGlobalizations)
                .HasForeignKey(ns=>ns.JobScopeId);
        }
    }
}
