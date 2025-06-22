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
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Id)
                .UseIdentityColumn();

            builder.HasOne(x => x.Role)
              .WithMany(x => x.Employees)
              .HasForeignKey(x => x.RoleId);

            builder.HasOne(x => x.Image)
             .WithMany(x => x.Employees)
             .HasForeignKey(x => x.ImageId);

            builder.HasOne(x => x.Ward)
             .WithMany(x => x.Users)
             .HasForeignKey(x => x.WardId);

            builder.HasIndex(c => c.Username)
                .IsUnique();

            builder.HasIndex(c => c.Phone)
                .IsUnique();

            builder.HasIndex(c => c.Cccd)
                .IsUnique();

            builder.HasIndex(c => c.Email)
                .IsUnique();

            //builder.Property(c => c.Password)
            //  .IsRequired()
            //  .HasMaxLength(100);

            builder.Property(c => c.UrlName)
              .IsRequired()
              .HasMaxLength(100);

            builder.Property(c => c.Name)
              .IsRequired()
              .HasMaxLength(100);


            //builder.HasOne(x => x.CreatedByUser)
            //  .WithMany(x => x.CreatedUser)
            //  .HasForeignKey(x => x.CreatedBy)
            //  .OnDelete(DeleteBehavior.ClientSetNull);

            //builder.HasOne(x => x.UpdatedByUser)
            //  .WithMany(x => x.UpdatedUser)
            //  .HasForeignKey(x => x.UpdatedBy)
            //  .OnDelete(DeleteBehavior.ClientSetNull); 
        }
    }
}
