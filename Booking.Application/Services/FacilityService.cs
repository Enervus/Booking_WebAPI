using AutoMapper;
using Booking.Application.Resources;
using Booking.Domain.Dto.Facility;
using Booking.Domain.Entity;
using Booking.Domain.Enum;
using Booking.Domain.Interfaces.Repositories;
using Booking.Domain.Interfaces.Services;
using Booking.Domain.Interfaces.Validations;
using Booking.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Serilog;


namespace Booking.Application.Services
{
    /// <inheritdoc/>
    internal class FacilityService : IFacilityService
    {
        private readonly IBaseRepository<Facility> _facilityRepository;

        private readonly IBaseRepository<User> _userRepository;
        private readonly ILogger _logger;
        private readonly IFacilityValidator _facilityValidator;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _hostEnvironment;

        public FacilityService(IBaseRepository<Facility> facilityRepository, IBaseRepository<User> userRepository,
            ILogger logger, IFacilityValidator facilityValidator, IMapper mapper, IHostEnvironment hostEnvironment)
        {
            _facilityRepository = facilityRepository;
            _userRepository = userRepository;
            _logger = logger;
            _facilityValidator = facilityValidator;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }
        /// <inheritdoc/>
        public async Task<CollectionResult<FacilityDto>> GetFacilitiesAsync(long userId)
        {
            FacilityDto[] facilities;
            facilities = await _facilityRepository.GetAll()
                   .Where(x => x.UserId == userId)
                   .Select(x => new FacilityDto(x.Id, x.Name, x.FacilityType, x.Address, x.Cost,
                       x.Coefficient, x.Description, x.Status, x.ImgPath, x.ImgPath, x.CreatedAt.ToLongDateString(), x.CreatedBy))
                   .ToArrayAsync(); //ToArrayAsync тк он из EF Core

            if (!facilities.Any())
            {
                _logger.Warning(ErrorMessage.FacilitiesNotFound, facilities.Length);
                return new CollectionResult<FacilityDto>()
                {
                    ErrorMessage = ErrorMessage.FacilitiesNotFound,
                    ErrorCode = (int)ErrorCodes.FacilitiesNotFound
                };
            }

            return new CollectionResult<FacilityDto>()
            {
                Data = facilities,
                Count = facilities.Length
            };
        }


        /// <inheritdoc/>
        public async Task<BaseResult<FacilityDto>> GetFacilityByIdAsync(long id)
        {
            FacilityDto? facility;
            facility = _facilityRepository.GetAll()
                       .AsEnumerable()
                      .Select(x => new FacilityDto(x.Id, x.Name, x.FacilityType, x.Address, x.Cost,
                          x.Coefficient, x.Description, x.Status, x.ImgName, x.ImgPath, x.CreatedAt.ToLongDateString(), x.CreatedBy))
                      .FirstOrDefault(x => x.Id == id);//async убрали, так как Queryable не позволяет выполнить ToLongDateString()

            if (facility == null)
            {
                _logger.Warning($"Объект с id:{id} не найден");
                return new BaseResult<FacilityDto>()
                {
                    ErrorMessage = ErrorMessage.FacilityNotFound,
                    ErrorCode = (int)ErrorCodes.FacilityNotFound
                };
            }
            return new BaseResult<FacilityDto>()
            {
                Data = facility
            };
        }
        /// <inheritdoc/>
        public async Task<CollectionResult<FacilityDto>> GetFacilityByTypeAsync(int facilityType)
        {
            FacilityDto[] facilities;

            facilities = await _facilityRepository.GetAll()
                    .Where(x => x.FacilityType == (FacilityType)facilityType)
                    .Select(x => new FacilityDto(x.Id, x.Name, x.FacilityType, x.Address, x.Cost,
                        x.Coefficient, x.Description, x.Status, x.ImgName, x.ImgPath, x.CreatedAt.ToLongDateString(), x.CreatedBy))
                        .ToArrayAsync();

            if (!facilities.Any())
            {
                _logger.Warning($"Объект с id:{facilityType} не найден");
                return new CollectionResult<FacilityDto>()
                {
                    ErrorMessage = ErrorMessage.FacilitiesNotFound,
                    ErrorCode = (int)ErrorCodes.FacilitiesNotFound
                };
            }
            return new CollectionResult<FacilityDto>()
            {
                Data = facilities,
                Count = facilities.Length
            };
        }
        /// <inheritdoc/>
        public async Task<BaseResult<FacilityDto>> CreateFacilityAsync(CreateFacilityDto dto)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.UserId);
            var facility = await _facilityRepository.GetAll().FirstOrDefaultAsync(x => x.Name == dto.Name);
            var result = _facilityValidator.CreateValidator(facility, user);
            if (!result.IsSuccess)
            {
                return new BaseResult<FacilityDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode
                };
            }
            string path;
            if (dto.img == null || dto.img.Length == 0)
            {
                path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\images\\default.jpg");
            }
            else
            {
                string directoryPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\images", user.Login);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\images", user.Login, dto.img.FileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await dto.img.CopyToAsync(stream);
                    stream.Close();
                }
            }
            facility = new Facility()
            {
                Name = dto.Name,
                Address = dto.Address,
                Coefficient = dto.Coefficient,
                Cost = dto.Cost,
                FacilityType = (FacilityType)dto.FacilityType,
                Description = dto.Description,
                Status = (Status)dto.Status,
                ImgName = dto.img.FileName,
                ImgPath = path,
                UserId = user.Id,
                CreatedBy = user.Id
            };
            await _facilityRepository.CreateAsync(facility);
            await _facilityRepository.SaveChangesAsync();
            return new BaseResult<FacilityDto>()
            {
                Data = _mapper.Map<FacilityDto>(facility)
            };
        }

        /// <inheritdoc/>
        public async Task<BaseResult<FacilityDto>> DeleteFacilityAsync(long id)
        {
            var facility = await _facilityRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);
            var result = _facilityValidator.ValidateOnNull(facility);
            if (!result.IsSuccess)
            {
                return new BaseResult<FacilityDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = (int)result.ErrorCode
                };
            }

            _facilityRepository.Remove(facility);
            await _facilityRepository.SaveChangesAsync();
            return new BaseResult<FacilityDto>()
            {
                Data = _mapper.Map<FacilityDto>(facility)
            };

        }

        /// <inheritdoc/>
        public async Task<BaseResult<FacilityDto>> UpdateFacilityAsync(UpdateFacilityDto dto)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.UserId);
            if (user == null)
            {
                _logger.Warning(ErrorMessage.UserNotFound);
                return new BaseResult<FacilityDto>()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }
            var facility = await _facilityRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);
            var result = _facilityValidator.ValidateOnNull(facility);
            if (!result.IsSuccess)
            {
                _logger.Warning(result.ErrorMessage);
                return new BaseResult<FacilityDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode
                };
            }

            facility.Name = dto.Name;
            facility.Coefficient = dto.Coefficient;
            facility.Cost = dto.Cost;
            facility.Description = dto.Description;
            facility.Status = (Status)dto.Status;
            facility.UpdatedBy = user.Id;

            var updatedFacility = _facilityRepository.Update(facility);
            await _facilityRepository.SaveChangesAsync();
            return new BaseResult<FacilityDto>()
            {
                Data = _mapper.Map<FacilityDto>(updatedFacility)
            };
        }
        /// <inheritdoc/>
        public async Task<CollectionResult<FacilityDto>> GetAllFacilitiesAsync()
        {
            FacilityDto[] facilities;
            facilities = await _facilityRepository.GetAll()
                .Select(x => new FacilityDto(x.Id, x.Name, x.FacilityType, x.Address, x.Cost, x.Coefficient,
                        x.Description, x.Status, x.ImgName, x.ImgPath, x.CreatedAt.ToLongDateString(), x.CreatedBy))
                .ToArrayAsync();

            if (!facilities.Any())
            {
                _logger.Warning(ErrorMessage.FacilitiesNotFound, facilities.Length);
                return new CollectionResult<FacilityDto>()
                {
                    ErrorMessage = ErrorMessage.FacilitiesNotFound,
                    ErrorCode = (int)ErrorCodes.FacilitiesNotFound
                };
            }
            return new CollectionResult<FacilityDto>()
            {
                Data = facilities,
                Count = facilities.Length
            };
        }
    }
}
