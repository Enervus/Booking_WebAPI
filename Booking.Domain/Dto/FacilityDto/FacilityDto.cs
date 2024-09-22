using Booking.Domain.Enum;

namespace Booking.Domain.Dto.Facility
{
    public record FacilityDto(long Id, string Name, FacilityType FacilityType, string Address, int Cost ,
        double Coefficient, string Description,Status Status,string ImgName, string ImgPath, string DateCreated,long CreatedBy);
}
