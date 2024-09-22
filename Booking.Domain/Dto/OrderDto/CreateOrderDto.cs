using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Dto.Order
{
    public record CreateOrderDto(long UserId,long TotalCost,int[] TimeOfLease, long[] FacilityIds);
}
