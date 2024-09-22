using Booking.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace Booking.Domain.Dto.Facility;

public record CreateFacilityDto(string Name, int FacilityType, string Address, int Cost ,
    double Coefficient, string Description, int Status, int PostStatus, IFormFile img,long UserId);