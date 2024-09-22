using AutoMapper;
using Booking.Domain.Dto.Order;
using Booking.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Mapping
{
    public class OrderMaping:Profile
    {
        public OrderMaping()
        {
            CreateMap<Order, OrderDto>()
                 .ForCtorParam("Id", o => o.MapFrom(s => s.Id))
                .ForCtorParam("UserId", o => o.MapFrom(s => s.UserId))
                .ForCtorParam("FacilityIds", o => o.MapFrom(s => s.Facilities.Select(x => x.Id)))
                .ForCtorParam("FacilitiesNames", o => o.MapFrom(s=>s.Facilities.Select(x=>x.Name)))
                .ReverseMap();
        }
    }
}
