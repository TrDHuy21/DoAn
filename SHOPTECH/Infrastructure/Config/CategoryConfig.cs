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
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Id)
                .UseIdentityColumn();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}
