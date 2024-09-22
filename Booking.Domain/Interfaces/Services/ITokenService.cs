using Booking.Domain.Dto.User;
using Booking.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    }
}
