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
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Id)
                .UseIdentityColumn();

            builder.HasOne(c => c.MainImage)
                .WithMany(i => i.Products)
                .HasForeignKey(c => c.MainImageId);

            builder.HasOne(x => x.Brand)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.BrandId);

            builder.HasOne(x => x.Category)
               .WithMany(x => x.Products)
               .HasForeignKey(x => x.CategoryId);

            builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);
            builder.HasIndex(c => c.Name)
                .IsUnique();



            //builder.HasOne(x => x.CreatedByUser)
            //  .WithMany(x => x.CreatedProduct)
            //  .HasForeignKey(x => x.CreatedBy)
            //  .OnDelete(DeleteBehavior.ClientSetNull);

            //builder.HasOne(x => x.UpdatedByUser)
            //  .WithMany(x => x.UpdatedProduct)
            //  .HasForeignKey(x => x.UpdatedBy)
            //  .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
