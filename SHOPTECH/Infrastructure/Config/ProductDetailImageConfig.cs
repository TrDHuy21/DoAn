using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enity;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class ProductDetailImageConfig : IEntityTypeConfiguration<ProductDetailImage>
    {
        public void Configure(EntityTypeBuilder<ProductDetailImage> builder)
        {
            builder.ToTable("ProductDetailImages");
            builder.HasKey(x => new { x.ProductDetailId, x.ImageFileId });

            builder.HasOne(x => x.ProductDetail)
                .WithMany(x => x.ProductDetailImages)
                .HasForeignKey(x => x.ProductDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            builder.HasOne(x => x.ImageFile)
                .WithMany(x => x.ProductDetailImages)
                .HasForeignKey(x => x.ImageFileId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
