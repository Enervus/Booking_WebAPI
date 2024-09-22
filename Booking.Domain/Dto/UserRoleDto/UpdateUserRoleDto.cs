using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Dto.UserRoleDto
{
    public record UpdateUserRoleDto(string Login, long OldRoleId, long NewRoleId);
}
