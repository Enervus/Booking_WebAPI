using Booking.Domain.Dto.User;
using Booking.Domain.Interfaces.Services;
using Booking.Domain.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        ///    Sample request:
        ///    
        ///        POST
        ///        {
        ///         "login": "string",
        ///         "password": "string",
        ///         "passwordConfirm": "string",
        ///         "roleId": 0
        ///         }
        /// </remarks>
        /// <response code = "200">Если пользователь успешно зарегистрировался</response>
        /// <response code = "400">Если пользователь не смог зарегистрироваться</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<UserDto>>> Register([FromBody] RegisterUserDto dto)
        {
            var response = await _authService.Register(dto);
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
        /// Логин пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        ///    Sample request:
        ///    
        ///        POST
        ///        {
        ///         "login": "string",
        ///         "password": "string"
        ///         }
        /// </remarks>
        /// <response code = "200">Если пользователь успешно залогинился</response>
        /// <response code = "400">Если пользователь не смог залогиниться</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<TokenDto>>> Login([FromBody] LoginUserDto dto)
        {
            
            var response = await _authService.Login(dto);
            if (response.IsSuccess)
            {
                HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", response.Data.RefreshToken,
                new CookieOptions
                {
                     MaxAge = TimeSpan.FromMinutes(60)
                });
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
