using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Enum
{
    public enum Status
    {
        [Display(Name = "Доступен")]
        Available = 1,
        [Display(Name = "Занят")]
        Taken = 2
    }
}
