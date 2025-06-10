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
    public class ProductAttributeConfig : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.ToTable("ProductAttribute");
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Id)
                .UseIdentityColumn();

            builder.HasOne(x => x.Category)
                .WithMany(x => x.ProductAttributes)
                .HasForeignKey(x => x.CategoryId);

            builder.Property(c => c.UrlName)
              .IsRequired()
              .HasMaxLength(100);

            builder.Property(c => c.Name)
              .IsRequired()
              .HasMaxLength(100);

            //builder.HasOne(x => x.CreatedByUser)
            //  .WithMany(x => x.CreatedProductAttribute)
            //  .HasForeignKey(x => x.CreatedBy)
            //  .OnDelete(DeleteBehavior.ClientSetNull);

            //builder.HasOne(x => x.UpdatedByUser)
            //  .WithMany(x => x.UpdatedProductAttribute)
            //  .HasForeignKey(x => x.UpdatedBy)
            //  .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
