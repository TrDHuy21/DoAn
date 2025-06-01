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
    public class OrderDetailConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetail");
            builder.HasKey(x => new {x.OrderId, x.ProductDetailId});

            builder.HasOne(x => x.Order)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.OrderId);

            builder.HasOne(x => x.ProductDetail)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.ProductDetailId);

        }
    }
}
