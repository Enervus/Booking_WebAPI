using Booking.Domain.Dto.User;
using Booking.Domain.Interfaces.Repositories;
using Booking.Domain.Interfaces.Services;
using Booking.Domain.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TokenController : ControllerBase
    {
 
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Обновление токенов
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<ActionResult<BaseResult<TokenDto>>> RefreshToken([FromBody]TokenDto dto)
        {
            var response = await _tokenService.RefreshToken(dto);
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
