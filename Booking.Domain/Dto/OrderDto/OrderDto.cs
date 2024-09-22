using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Booking.Domain.Dto.Order
{
    public record OrderDto(long Id,long TotalCost,long UserId,List<long> FacilityIds,List<string> FacilitiesNames);
}
