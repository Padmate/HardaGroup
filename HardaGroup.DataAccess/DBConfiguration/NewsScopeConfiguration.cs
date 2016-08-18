using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess.DBConfiguration
{
    public class NewsScopeConfiguration : EntityTypeConfiguration<NewsScope>
    {
        internal NewsScopeConfiguration()
        {
            this.HasKey(m => m.Id);
            this.Property(m => m.TypeCode).HasMaxLength(100);
            this.Property(m => m.TypeName).HasMaxLength(200);
            this.Property(m => m.Culture).HasMaxLength(50);

        }
    }
}
