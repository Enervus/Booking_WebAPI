using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Booking.Domain.Enum
{
    public enum PostStatus
    {
        [Display(Name = "В обработке")]
        Verification = 1,
        [Display(Name = "Одобрен")]
        Approved = 2,
        [Display(Name = "Отказано")]
        Refused = 3
    }
}
