using Booking.Domain.Dto.UserDto;
using Booking.Domain.Interfaces.Services;
using Booking.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers.v1
{
    [Authorize(Roles = "Admin")]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        /// <response code = "200">Успешное выполнение</response>
        /// <response code = "400">Ошибка API</response>
        /// 
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<UserAllInfoDto>>> GetAllUsersAsync()
        {
            var response = await _userService.GetAllIUsersAsync();

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Получение пользователя с указанием идентификатора
        /// </summary>
        /// <response code = "200">Успешное выполнение</response>
        /// <response code = "400">Ошибка API</response>
        /// 
        [HttpGet(nameof(id))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<UserAllInfoDto>>> GetUserByIdAsync(long id)
        {
            var response = await _userService.GetUserByIdAsync(id);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
