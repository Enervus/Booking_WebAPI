using Booking.Domain.Dto;
using Booking.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Domain.Dto.Facility;
using Microsoft.AspNetCore.Http;

namespace Booking.Domain.Interfaces.Services
{
    /// <summary>
    ///  Сервис, отвечающий за работу с доменной частью объекта (Facility)
    /// </summary>
    public interface IFacilityService
    {
        /// <summary>
        /// Получение всех объектов пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CollectionResult<FacilityDto>> GetFacilitiesAsync(long userId);
        /// <summary>
        /// Получение объекта по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<FacilityDto>> GetFacilityByIdAsync(long id);
        /// <summary>
        /// Получение объекта по его типу
        /// </summary>
        /// <param name="facilityType"></param>
        /// <returns></returns>
        Task<CollectionResult<FacilityDto>> GetFacilityByTypeAsync(int facilityType);
        /// <summary>
        /// Получение всех объектов
        /// </summary>
        /// <param name="facilityType"></param>
        /// <returns></returns>
        Task<CollectionResult<FacilityDto>> GetAllFacilitiesAsync();
        /// <summary>
        /// Создание объекта с базовыми параметрами
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<FacilityDto>> CreateFacilityAsync(CreateFacilityDto dto);
        /// <summary>
        /// Удаление объекта по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult<FacilityDto>> DeleteFacilityAsync(long id);
        /// <summary>
        /// Обновление данных об объекте 
        /// /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<FacilityDto>> UpdateFacilityAsync(UpdateFacilityDto dto);
    }
}
