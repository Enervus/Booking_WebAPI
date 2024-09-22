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
    public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(2000);
            builder.Property(x => x.Address).IsRequired();
            builder.Property(x => x.Cost).IsRequired();
            builder.Property(x => x.Coefficient).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.PostStatus).IsRequired();
            builder.Property(x => x.ImgName).IsRequired();
            builder.Property(x => x.ImgPath).IsRequired();

        }
    }
}
