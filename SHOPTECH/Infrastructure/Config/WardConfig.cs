using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Enity;

namespace Infrastructure.Config
{
    public class WardConfig : IEntityTypeConfiguration<Ward>
    {
        public void Configure(EntityTypeBuilder<Ward> builder)
        {
            builder.ToTable("Ward");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.District)
                .WithMany(x => x.Wards)
                .HasForeignKey(x => x.DistrictId);
        }
    }
}
