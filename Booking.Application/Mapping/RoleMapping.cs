using AutoMapper;
using Booking.Domain.Dto.RoleDto;
using Booking.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Mapping
{
    public class RoleMapping:Profile
    {
        public RoleMapping()
        {
            CreateMap<Role, RoleDto>()
                .ForCtorParam("Id", o => o.MapFrom(s => s.Id))
                .ForCtorParam("Name", o => o.MapFrom(s => s.Name))
                .ReverseMap();
        }
    }
}
