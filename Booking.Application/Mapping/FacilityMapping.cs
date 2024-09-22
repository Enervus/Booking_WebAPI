using AutoMapper;
using Booking.Domain.Dto.Facility;
using Booking.Domain.Entity;

namespace Booking.Application.Mapping;

public  class FacilityMapping:Profile
{
    public  FacilityMapping()
    {
        CreateMap<Facility, FacilityDto>()
            .ForCtorParam("Id", o => o.MapFrom(s => s.Id))
            .ForCtorParam("Name", o => o.MapFrom(s => s.Name))
            .ForCtorParam("FacilityType", o => o.MapFrom(s => s.FacilityType))
            .ForCtorParam("Address", o => o.MapFrom(s => s.Address))
            .ForCtorParam("Cost", o => o.MapFrom(s => s.Cost))
            .ForCtorParam("Coefficient", o => o.MapFrom(s => s.Coefficient))
            .ForCtorParam("Description", o => o.MapFrom(s => s.Description))
            .ForCtorParam("Status", o => o.MapFrom(s => s.Status))
            .ForCtorParam("DateCreated", o => o.MapFrom(s => s.CreatedAt))
            .ReverseMap();
    }
}