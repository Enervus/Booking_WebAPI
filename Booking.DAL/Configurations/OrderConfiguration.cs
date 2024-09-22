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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany<Facility>(x => x.Facilities)
                .WithMany(o => o.Orders)
                .UsingEntity<OrderFacility>(
                j => j
                .HasOne<Facility>()
                .WithMany()
                .HasForeignKey(oh => oh.FacilityId),
                j => j
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey(oh => oh.OrderId));

        }
    }
}
