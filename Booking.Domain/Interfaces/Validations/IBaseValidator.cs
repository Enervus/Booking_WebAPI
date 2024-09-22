using Booking.Domain.Result;

namespace Booking.Domain.Interfaces.Validations;

public interface IBaseValidator<in T> where T:class
{
    BaseResult ValidateOnNull(T model);
}