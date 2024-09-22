using AutoMapper;
using Booking.Application.Resources;
using Booking.Domain.Dto.RoleDto;
using Booking.Domain.Dto.UserRoleDto;
using Booking.Domain.Entity;
using Booking.Domain.Enum;
using Booking.Domain.Interfaces.Databases;
using Booking.Domain.Interfaces.Repositories;
using Booking.Domain.Interfaces.Services;
using Booking.Domain.Result;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Services
{
    /// <inheritdoc/>
    public class RoleService : IRoleService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly IMapper _mapper;
        public RoleService(IBaseRepository<User> userRepository, IBaseRepository<Role> roleRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto dto)
        {
            var user = await _userRepository.GetAll()
                .Include(x=>x.Roles)
                .FirstOrDefaultAsync(x=>x.Login == dto.Login);

            if (user == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }

            var roles = user.Roles.Select(x => x.Name).ToArray();
            if(roles.All(x=>x != dto.RoleName))
            {
                var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == dto.RoleName);
                if(role == null)
                {
                    return new BaseResult<UserRoleDto>()
                    {
                        ErrorMessage = ErrorMessage.RoleNotFound,
                        ErrorCode = (int)ErrorCodes.RoleNotFound
                    };
                }

                UserRole userRole = new UserRole()
                {
                    RoleId = role.Id,
                    UserId = user.Id
                };
                await  _userRoleRepository.CreateAsync(userRole);

                return new BaseResult<UserRoleDto>()
                {
                    Data = new UserRoleDto(user.Login,role.Name)
                };
            }

            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.UserAlreadyExistsThisRole,
                ErrorCode = (int)ErrorCodes.UserAlreadyExistsThisRole
            };
        }

        /// <inheritdoc/>
        public async Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == dto.Name);  
            if(role!=null)///перенести все проверки в валидатор
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleAlreadyExists,
                    ErrorCode = (int)ErrorCodes.RoleAlreadyExists
                };
            }

            role = new Role()
            {
                Name = dto.Name
            };
            await _roleRepository.CreateAsync(role);
            await _roleRepository.SaveChangesAsync();
            return new BaseResult<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(role)
            };
        }
        /// <inheritdoc/>
        public async Task<BaseResult<RoleDto>> DeleteRoleAsync(long id)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)///перенести все проверки в валидатор
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCodes.RoleNotFound
                };
            }

            _roleRepository.Remove(role);
            await _roleRepository.SaveChangesAsync();

            return new BaseResult<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(role)
            };
        }

        /// <inheritdoc/>
        public async Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(DeleteUserRoleDto dto)
        {
            var user = await _userRepository.GetAll()
               .Include(x => x.Roles)
               .FirstOrDefaultAsync(x => x.Login == dto.Login);

            if (user == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }

            var role = user.Roles.FirstOrDefault(x => x.Id == dto.RoleId);

            if (role == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCodes.RoleNotFound
                };
            }

            var userRole = await _userRoleRepository.GetAll()
                .Where(x => x.RoleId == role.Id)
                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            _userRoleRepository.Remove(userRole);
            await _userRoleRepository.SaveChangesAsync();

            return new BaseResult<UserRoleDto>()
            {
                Data = new UserRoleDto(user.Login, role.Name)
            };
        }


        public async Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto dto)
        {
            var user = await _userRepository.GetAll()
               .Include(x => x.Roles)
               .FirstOrDefaultAsync(x => x.Login == dto.Login);

            if (user == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }

            var oldRole = user.Roles.FirstOrDefault(x => x.Id == dto.OldRoleId);

            if (oldRole == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCodes.RoleNotFound
                };
            }

            var newRole = _roleRepository.GetAll().FirstOrDefault(x => x.Id == dto.NewRoleId);

            if (newRole == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCodes.RoleNotFound
                };
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var userRole = await _userRoleRepository.GetAll()
                        .Where(x => x.RoleId == oldRole.Id)
                        .FirstAsync(x => x.UserId == user.Id);

                    _unitOfWork.UserRoles.Remove(userRole);
                    await _unitOfWork.SaveChangesAsync();

                    var newUserRole = new UserRole()
                    {
                        UserId = user.Id,
                        RoleId = newRole.Id
                    };

                    await _unitOfWork.UserRoles.CreateAsync(newUserRole);
                    await _unitOfWork.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }
            return new BaseResult<UserRoleDto>()
            {
                Data = new UserRoleDto(user.Login, newRole.Name)
            };
        }

        /// <inheritdoc/>
        public async Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto dto)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x=>x.Id == dto.Id);
            if (role == null)///перенести все проверки в валидатор
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCodes.RoleNotFound
                };
            }
            role.Name = dto.Name;
            var updatedRrole = _roleRepository.Update(role);
            await _roleRepository.SaveChangesAsync();
            return new BaseResult<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(updatedRrole)
            };

        }
    }
}
