using Booking.Domain.Dto.Facility;
using Booking.Domain.Interfaces.Services;
using Booking.Domain.Result;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers.v1
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FacilityController : ControllerBase
    {
        private readonly IFacilityService _facilityService;
        public FacilityController(IFacilityService facilityService)
        {
            _facilityService = facilityService;
        }

        /// <summary>
        /// Получение объекта с указанием идентификатора
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        ///    Sample request:
        ///    
        ///        GET
        ///        {
        ///            "id": 1,
        ///        }
        /// </remarks>
        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<FacilityDto>>> GetFacility(long id)//если бы возвращали IActionResult в ProducesResponseType нужно было бы указать возвращемый тип 
        {
            var response = await _facilityService.GetFacilityByIdAsync(id);
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
        /// Получение объектов с указанием типа
        /// </summary>
        /// <param name="facilityType"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET
        ///     {
        ///         "facilityType": 1,
        ///     }
        /// </remarks>
        ///   /// <response code = "200">Успешное выполнение</response>
        /// <response code = "400">Ошибка API</response>
        [MapToApiVersion("1.0")]
        [HttpGet(nameof(facilityType))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<FacilityDto>>> GetFacilitiesByType(int facilityType)
        {
            var response = await _facilityService.GetFacilityByTypeAsync(facilityType);
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
        /// Получение объектов, принадлежащих пользователю (с указанием его идентификатора) 
        /// </summary>
        /// <param name="userId"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET
        ///     {
        ///         "userId": 1,
        ///     }
        /// </remarks>
        /// <response code = "200">Успешное выполнение</response>
        /// <response code = "400">Ошибка API</response>
        [MapToApiVersion("1.0")]
        [HttpGet(nameof(userId))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<FacilityDto>>> GetUserFacilities(long userId)
        {
            var response = await _facilityService.GetFacilitiesAsync(userId);
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
        /// Получение всех объектов
        /// </summary>
        /// <response code = "200">Успешное выполнение</response>
        /// <response code = "400">Ошибка API</response>
        /// 
        [MapToApiVersion("1.0")]
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<FacilityDto>>> GetAllFacilities()
        {
            var response = await _facilityService.GetAllFacilitiesAsync();
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
        /// Удаление объекта с указанием идентификатора
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        ///    Sample request:
        ///    
        ///        DELETE
        ///        {
        ///            "id": 1,
        ///        }
        /// </remarks>
        /// <response code = "200">Если удаление объекта выполнилось</response>
        /// <response code = "400">Если удаление объекта не выполнилось</response>
        [Authorize(Roles = "Landlord,Admin")]
        [MapToApiVersion("1.0")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<FacilityDto>>> Delete(long id)
        {
            var response = await _facilityService.DeleteFacilityAsync(id);
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
        /// Создание объекта
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST
        ///     { 
        ///        "name" = "Facility№1",
        ///        "facilityType" = 1,
        ///        "address" = "test",
        ///        "status" = 1
        ///        "cost" = 11111,
        ///        "coefficient" = 1,
        ///        "description" =  "Test",
        ///        "postStatus" = 1,
        ///        "userId": 1
        ///     }
        /// </remarks>
        /// <response code = "200">Если объект создался</response>
        /// <response code = "400">Если объект не создался</response>
        [Authorize(Roles = "Landlord,Admin")]
        [MapToApiVersion("1.0")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<FacilityDto>>> Create([FromForm] CreateFacilityDto dto)
        {
            var response = await _facilityService.CreateFacilityAsync(dto);
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
        /// Обновление объекта с указанеием основных свойств
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT
        ///     {
        ///        "id": 1
        ///        "name" = "Facility№2",
        ///        "status" = 0,
        ///        "cost" = 222222,
        ///        "coefficient" = 2,
        ///        "description" =  "Test_2",
        ///        "postStatus" = 0,
        ///     }
        /// </remarks>
        /// <response code = "200">Если обновление данных объекта выполнилось</response>
        /// <response code = "400">Если обновление данных объекта не выполнилось</response>
        [Authorize(Roles = "Landlord,Admin")]
        [MapToApiVersion("1.0")]
        [HttpPut]// у пута в одном и том же запросе результат всегда один и тот же, пост создает каждый новый (одинаковый) запрос новый объект
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<FacilityDto>>> Update([FromBody] UpdateFacilityDto dto)
        {
            var response = await _facilityService.UpdateFacilityAsync(dto);
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
