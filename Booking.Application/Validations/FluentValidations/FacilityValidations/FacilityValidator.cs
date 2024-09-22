using System.Runtime.InteropServices.JavaScript;
using Booking.Application.Resources;
using Booking.Domain.Entity;
using Booking.Domain.Enum;
using Booking.Domain.Interfaces.Validations;
using Booking.Domain.Result;

namespace Booking.Application.Validations.FluentValidations.Facility;

public class FacilityValidator : IFacilityValidator
{
    public BaseResult ValidateOnNull(Domain.Entity.Facility model)
    {
        if (model == null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.FacilityNotFound,
                ErrorCode = (int)ErrorCodes.FacilityNotFound
            };
        }

        return new BaseResult();
    }

    public BaseResult CreateValidator(Domain.Entity.Facility facility, User user)
    {
        if (facility != null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.FacilityAlreadyExists,
                ErrorCode = (int)ErrorCodes.FacilityAlreadyExists
            };
        }

        if (user == null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = (int)ErrorCodes.UserNotFound
            };
        }
        return new BaseResult();
    }
}