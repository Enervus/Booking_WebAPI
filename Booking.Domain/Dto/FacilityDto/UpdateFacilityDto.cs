using Booking.Domain.Enum;

namespace Booking.Domain.Dto.Facility;


public record UpdateFacilityDto(long Id,string Name, int Cost ,
    double Coefficient, string Description, int Status, int PostStatus, long UserId);