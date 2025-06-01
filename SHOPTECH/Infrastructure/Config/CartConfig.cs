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
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");
            builder.HasKey(x => new {x.ProductDetailId, x.UserId});

            builder.HasOne(x => x.ProductDetail)
             .WithMany(x => x.Carts)
             .HasForeignKey(x => x.ProductDetailId)
             .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(x => x.User)
             .WithMany(x => x.Carts)
             .HasForeignKey(x => x.UserId) ;

            //quantity is required, min 0
            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.ToTable(t => t.HasCheckConstraint("CK_Cart_Quantity_NonNegative", "[Quantity] >= 0"));



        }
    }
}
