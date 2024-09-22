using Booking.Domain.Entity;
using Booking.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DAL.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.HasData(new List<Role>()
            {
                new Role()
                {
                    Id = 1,
                    Name = nameof(Roles.Renter),
                },
                new Role()
                {
                    Id = 2,
                    Name = nameof(Roles.Landlord),
                },
                new Role()
                {
                    Id = 3,
                    Name = nameof(Roles.Admin),
                },
            });
        }
    }
}
