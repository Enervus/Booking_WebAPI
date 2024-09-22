using Booking.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DAL.Configurations
{
    public class OrderFacilityConfiguration : IEntityTypeConfiguration<OrderFacility>
    {
        public void Configure(EntityTypeBuilder<OrderFacility> builder)
        {
        }
    }
}
