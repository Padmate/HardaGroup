using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess.DBConfiguration
{
    public class AboutConfiguration:EntityTypeConfiguration<About>
    {
        internal AboutConfiguration()
        {
            this.HasKey(m => m.Id);
            this.Property(m => m.TypeCode).HasMaxLength(100);

        }
    }
}
