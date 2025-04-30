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
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Id)
                .UseIdentityColumn();

            builder.HasOne(x => x.Ward)
             .WithMany(x => x.Orders)
             .HasForeignKey(x => x.WardId)
             .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(x => x.Employee)
            .WithMany(x => x.EmployeeOrders)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(x => x.Customer)
            .WithMany(x => x.CustomerOrders)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
