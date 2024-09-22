using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Booking.Domain.Entity;
using Booking.Domain.Enum;

namespace Booking.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)//настройка сущности
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Login).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Password).IsRequired();

            builder.HasMany<Facility>(x => x.Facilities)
                  .WithOne(x => x.User)
                  .HasForeignKey(x => x.UserId)
                  .HasPrincipalKey(x => x.Id);

            builder.HasMany<Order>(x => x.Orders)
                 .WithOne(x => x.User)
                 .HasForeignKey(x => x.UserId)
                 .HasPrincipalKey(x => x.Id);

            builder.HasMany(x => x.Roles)
                .WithMany(o => o.Users)
                .UsingEntity<UserRole>(
                j => j
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(oh => oh.RoleId),
                j => j
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(oh => oh.UserId));

        }
    }
}
