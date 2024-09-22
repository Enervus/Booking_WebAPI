using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Enum
{
    public enum FacilityType
    {
        [Display(Name = "Офис")]
        Office = 1,
        [Display(Name = "Номер в отеле")]
        HotelRoom = 2,
        [Display(Name = "Квартира")]
        Flat = 3
    }
}
