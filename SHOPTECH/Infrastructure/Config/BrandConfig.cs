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
    public class BrandConfig : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brand");
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Id)
                .UseIdentityColumn();

            // name is unique, max lengh 100
            builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(c => c.UrlName)
               .IsRequired()
               .HasMaxLength(100);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            //builder.HasOne(x => x.CreatedByUser)
            //  .WithMany(x => x.CreatedBrand)
            //  .HasForeignKey(x => x.CreatedBy)
            //  .OnDelete(DeleteBehavior.ClientSetNull);

            //builder.HasOne(x => x.UpdatedByUser)
            //  .WithMany(x => x.UpdatedBrand)
            //  .HasForeignKey(x => x.UpdatedBy)
            //  .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
