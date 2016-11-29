using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess.DBConfiguration
{
    public class MailConfiguration : EntityTypeConfiguration<Mail>
    {
        internal MailConfiguration()
        {
            this.HasKey(m => m.Id);
            this.Property(m => m.Creator).HasMaxLength(50);
            this.Property(m => m.Modifier).HasMaxLength(50);
            this.Property(m => m.Subject).HasMaxLength(2000);

        }
    }
}
