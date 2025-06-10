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
    public class ProductDetailConfig : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.ToTable("ProductDetail");
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Id)
                .UseIdentityColumn();

            builder.HasOne(x => x.Product)
               .WithMany(x => x.ProductDetails)
               .HasForeignKey(x => x.ProductId)
              .OnDelete(DeleteBehavior.ClientSetNull);


            builder.HasOne(x => x.Image)
               .WithMany(x => x.ProductDetails)
               .HasForeignKey(x => x.ImageId);

            builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(c => c.Code)
              .IsRequired()
              .HasMaxLength(20);

            builder.Property(c => c.UrlName)
              .IsRequired()
              .HasMaxLength(100);

            builder.Property(c => c.ColorCode)
              .IsRequired()
              .HasMaxLength(10);

            builder.Property(c => c.ColorName)
              .IsRequired()
              .HasMaxLength(20);
            //builder.HasOne(x => x.CreatedByUser)
            //  .WithMany(x => x.CreatedProductDetail)
            //  .HasForeignKey(x => x.CreatedBy)
            //  .OnDelete(DeleteBehavior.ClientSetNull);

            //builder.HasOne(x => x.UpdatedByUser)
            //  .WithMany(x => x.UpdatedProductDetail)
            //  .HasForeignKey(x => x.UpdatedBy)
            //  .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
