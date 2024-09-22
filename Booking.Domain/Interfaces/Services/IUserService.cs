using Booking.Domain.Dto.User;
using Booking.Domain.Dto.UserDto;
using Booking.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Interfaces.Services
{
    /// <summary>
    ///  Сервис, отвечающий за работу с данными пользователя
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        /// <returns></returns>
        Task<CollectionResult<UserAllInfoDto>> GetAllIUsersAsync();
        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<UserAllInfoDto>> GetUserByIdAsync(long id);
    }
}
