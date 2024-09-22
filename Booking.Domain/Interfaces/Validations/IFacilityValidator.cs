using Booking.Domain.Entity;
using Booking.Domain.Result;

namespace Booking.Domain.Interfaces.Validations;

public interface IFacilityValidator:IBaseValidator<Facility>
{
    /// <summary>
    /// Проверяется наличие объекта, если объект с переданным названием существует, то создать такой же объект нельзя
    /// Проверяется наличие пользователя, если с UserId пользователь не найден, то такого пользователя нет
    /// </summary>
    /// <param name="facility"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    BaseResult CreateValidator(Facility facility, User user);
}