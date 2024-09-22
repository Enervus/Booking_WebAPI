using AutoMapper;
using Booking.Application.Resources;
using Booking.Domain.Dto.RoleDto;
using Booking.Domain.Dto.UserDto;
using Booking.Domain.Entity;
using Booking.Domain.Enum;
using Booking.Domain.Interfaces.Repositories;
using Booking.Domain.Interfaces.Services;
using Booking.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Facility> _facilityRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        /// <inheritdoc/>
        public UserService(IBaseRepository<User> userRepository, IBaseRepository<Facility> facilityRepository, IBaseRepository<Role> roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _facilityRepository = facilityRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<CollectionResult<UserAllInfoDto>> GetAllIUsersAsync()
        {
            UserAllInfoDto[] users;
            RoleDto[] roles;
            users = await _userRepository.GetAll()
                .Select(x => new UserAllInfoDto(x.Id, x.Login, x.Roles.Select(x => x.Name).ToArray(), x.Facilities.Select(x => x.Name).ToArray(), x.Orders.Select(x => x.Id).ToArray(), x.CreatedAt, x.CreatedBy, x.UpdatedAt, x.UpdatedBy))
                .ToArrayAsync();
            if (!users.Any())
            {
                _logger.Warning(ErrorMessage.UsersNotFound);
                return new CollectionResult<UserAllInfoDto>()
                {
                   ErrorMessage = ErrorMessage.UsersNotFound,
                   ErrorCode = (int)ErrorCodes.UsersNotFound
                };
            }
            return new CollectionResult<UserAllInfoDto>()
            {
                 Data = users,
                 Count = users.Length
            };
        }

        /// <inheritdoc/>
        public async Task<BaseResult<UserAllInfoDto>> GetUserByIdAsync(long id)
        {
            var user = await _userRepository.GetAll()
                 .Select(x => new UserAllInfoDto(x.Id, x.Login, x.Roles.Select(x=>x.Name).ToArray(), x.Facilities.Select(x=>x.Name).ToArray(), x.Orders.Select(x=>x.Id).ToArray(), x.CreatedAt, x.CreatedBy, x.UpdatedAt, x.UpdatedBy))
                 .FirstOrDefaultAsync(x => x.Id == id);

            if(user == null)
            {
                _logger.Warning(ErrorMessage.UserNotFound);
                return new BaseResult<UserAllInfoDto>()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }

            return new BaseResult<UserAllInfoDto>()
            {
                Data = user
            };
        }
    }
}
