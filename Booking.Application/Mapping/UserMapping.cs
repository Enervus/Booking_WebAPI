using AutoMapper;
using Booking.Domain.Dto.Facility;
using Booking.Domain.Dto.User;
using Booking.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Mapping
{
    public class UserMapping:Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserDto>()
                .ForCtorParam("Login",o=>o.MapFrom(s=>s.Login))//изначально юыло только в Facility 
                .ReverseMap();
        }
    }
}
