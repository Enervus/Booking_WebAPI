using Booking.Domain.Dto.Order;
using Booking.Domain.Dto.OrderDto;
using Booking.Domain.Entity;
using Booking.Domain.Interfaces.Services;
using Booking.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Получение заказа с указанием идентификатора заказа
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
        /// <response code = "200">Успешное выполнение</response>
        /// <response code = "400">Ошибка API</response>
        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<OrderDto>>> GetOrder(long id)
        {
            var response = await _orderService.GetOrderByIdAsync(id);
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
        /// Получение объекта с указанием идентификатора пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <remarks>
        ///    Sample request:
        ///    
        ///        GET
        ///        {
        ///            "id": 1,
        ///        }
        /// </remarks>
        /// <response code = "200">Успешное выполнение</response>
        /// <response code = "400">Ошибка API</response>
        [MapToApiVersion("1.0")]
        [HttpGet(nameof(userId))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<OrderDto>>> GetUserOrders(long userId)
        {
            var response = await _orderService.GetOrdersAsync(userId);
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
        /// Создание заказа
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST
        ///     { 
        ///        "id": 1,
        ///        "totalCost": 0,
        ///         "TimeOfLease": [
        ///        7,
        ///        5,
        ///        6
        ///        ],
        ///        "facilityIds": [
        ///             1,
        ///             2,
        ///             3
        ///         ]
        ///     }
        /// </remarks>
        /// <response code = "200">Если заказ создался</response>
        /// <response code = "400">Если заказ не создался</response>
        [MapToApiVersion("1.0")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<OrderDto>>> Create([FromBody] CreateOrderDto dto)
        {
            var response = await _orderService.CreateOrderAsync(dto);
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
        /// Удаление заказа с указанием идентификатора
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
        /// <response code = "200">Если заказ удалился</response>
        /// <response code = "400">Если заказ не удалился</response>
        [MapToApiVersion("1.0")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<OrderDto>>> Delete(long id)
        {
            var response = await _orderService.DeleteOrderAsync(id);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else{
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Обнолвение заказа
        /// </summary>
        /// <param name="dto"></param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT
        ///     { 
        ///        "id": 1,
        ///        "totalCost": 0,
        ///        "TimeOfLease": [
        ///        7,
        ///        5,
        ///        6
        ///        ],
        ///        "facilityIds": [
        ///             1,
        ///             2,
        ///             3
        ///         ]
        ///     }
        /// </remarks>
        /// <response code = "200">Если заказ обновился</response>
        /// <response code = "400">Если заказ не обновился</response>
        [MapToApiVersion("1.0")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<OrderDto>>> Update([FromBody] UpdateOrderDto dto)
        {
            var response = await _orderService.UpdateOrderAsync(dto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else{
                return BadRequest(response);
            }
        }
    }
}
