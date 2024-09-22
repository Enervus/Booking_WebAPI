using AutoMapper;
using Booking.Application.Resources;
using Booking.Domain.Dto.User;
using Booking.Domain.Entity;
using Booking.Domain.Enum;
using Booking.Domain.Interfaces.Databases;
using Booking.Domain.Interfaces.Repositories;
using Booking.Domain.Interfaces.Services;
using Booking.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Booking.Application.Services
{
    /// <inheritdoc/>
    public class AuthService : IAuthService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserToken> _userTokenRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public AuthService(IBaseRepository<User> userRepository, ILogger logger, IMapper mapper, IBaseRepository<UserToken> userTokenRepository, ITokenService tokenService, IBaseRepository<Role> roleRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _tokenService = tokenService;
            _logger = logger;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }
        /// <inheritdoc/>
        public async Task<BaseResult<UserDto>> Register(RegisterUserDto dto)
        {
            if (dto.Password != dto.PasswordConfirm)
            {
                _logger.Warning(ErrorMessage.PasswordNotEqualsPasswordConfirm);
                return new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.PasswordNotEqualsPasswordConfirm,
                    ErrorCode = (int)ErrorCodes.PasswordNotEqualsPasswordConfirm
                };
            }
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == dto.Login);

            if (user != null)
            {
                _logger.Warning(ErrorMessage.UserAlreadyExists);
                return new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.UserAlreadyExists,
                    ErrorCode = (int)ErrorCodes.UserAlreadyExists
                };
            }

            var hashUserPassword = HashPassword(dto.Password);

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    user = new User()
                    {
                        Login = dto.Login,
                        Password = hashUserPassword
                    };

                    await _unitOfWork.Users.CreateAsync(user);

                    await _unitOfWork.SaveChangesAsync();

                    var role = await _unitOfWork.Roles.GetAll().FirstOrDefaultAsync(x => x.Id == dto.RoleId);

                    if (role == null)
                    {
                        return new BaseResult<UserDto>()
                        {
                            ErrorMessage = ErrorMessage.RoleNotFound,
                            ErrorCode = (int)ErrorCodes.RoleNotFound
                        };
                    }

                    UserRole userRole = new UserRole()
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };

                    await _unitOfWork.UserRoles.CreateAsync(userRole);

                    await _unitOfWork.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }

            return new BaseResult<UserDto>()
            {
                Data = _mapper.Map<UserDto>(user)
            };
        }
        /// <inheritdoc/>
        public async Task<BaseResult<TokenDto>> Login(LoginUserDto dto)
        {
            var user = await _userRepository.GetAll()
                   .Include(x => x.Roles)
                   .FirstOrDefaultAsync(x => x.Login == dto.Login);
            if (user == null)
            {
                _logger.Warning(ErrorMessage.UserNotFound);
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }

            if (!IsVerifyPassword(user.Password, dto.Password))
            {
                _logger.Warning(ErrorMessage.PasswordIsWrong);
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.PasswordIsWrong,
                    ErrorCode = (int)ErrorCodes.PasswordIsWrong
                };
            }

            var userToken = await _userTokenRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == user.Id);

            var userRoles = user.Roles;
            var claims = userRoles.Select(x => new Claim(ClaimTypes.Role, x.Name)).ToList();
            claims.Add(new Claim(ClaimTypes.Name, user.Login));
            claims.Add(new Claim("UserId",user.Id.ToString()));
            var refreshToken = _tokenService.GenerateRefreshToken();
            var accessToken = _tokenService.GenerateAccessToken(claims);

            if (userToken == null)
            {
                userToken = new UserToken()
                {
                    UserId = user.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7)
                };
                await _userTokenRepository.CreateAsync(userToken);
                await _userTokenRepository.SaveChangesAsync();
            }
            else
            {
                userToken.RefreshToken = refreshToken;
                userToken.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
                userToken = _userTokenRepository.Update(userToken);
                await _userTokenRepository.SaveChangesAsync();
            }

            return new BaseResult<TokenDto>()
            {
                Data = new TokenDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            };
        }

        private string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool IsVerifyPassword(string userPasswordHash, string userPassword)
        {
            var hash = HashPassword(userPassword);
            return userPasswordHash == hash;

        }
    }
}
