using Booking.Domain.Dto.User;
using Booking.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис предназначенный для авторизации/регистрации 
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto>> Register(RegisterUserDto dto);
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<TokenDto>> Login(LoginUserDto dto);
    }
}
