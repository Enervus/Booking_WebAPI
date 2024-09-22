using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Dto.UserDto
{
    public record UserAllInfoDto(long Id,string Login, string[] Roles, string[] Facilities,long[] OrderIds,DateTime CreatedAt, long CreatedBy, DateTime? UpdatedAt, long? UpdatedBy);
}
