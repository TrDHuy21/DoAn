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
    public class ProductDetailAttributeConfig : IEntityTypeConfiguration<ProductDetailAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductDetailAttribute> builder)
        {
            builder.ToTable("ProductDetailAttribute");
            builder.HasKey(x => new { x.ProductDetailId, x.ProductAttributeId });
           
            builder.HasOne(x => x.Image)
               .WithMany(x => x.ProductDetailAttributes)
               .HasForeignKey(x => x.ImageId);

            builder.HasOne(x => x.ProductDetail)
               .WithMany(x => x.ProductDetailAttributes)
               .HasForeignKey(x => x.ProductDetailId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(x => x.ProductAttribute)
               .WithMany(x => x.ProductDetailAttributes)
               .HasForeignKey(x => x.ProductAttributeId);
        }
    }
}
