using Booking.Domain.Entity;
using Booking.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Interfaces.Validations
{

    public interface IOrderValidator:IBaseValidator<Order>
    {
        /// <summary>
        /// Проверяется наличие пользователя, если с UserId пользователь не найден, то такого пользователя нет
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        BaseResult CreateValidator(User user, List<Facility> facilities);
    }
}
