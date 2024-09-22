using Booking.Domain.Dto.Order;
using Booking.Domain.Dto.OrderDto;
using Booking.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Interfaces.Services
{
    /// <summary>
    ///  Сервис, отвечающий за работу с доменной частью заказа (Order)
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Получение всех заказов пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<CollectionResult<OrderDto>> GetOrdersAsync(long userId);
        /// <summary>
        /// Получение заказа по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<BaseResult<OrderDto>> GetOrderByIdAsync(long id);
        /// <summary>
        /// Создание заказа с базовыми параметрами
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<BaseResult<OrderDto>> CreateOrderAsync(CreateOrderDto dto);
        /// <summary>
        /// Удаление заказа по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<BaseResult<OrderDto>> DeleteOrderAsync(long id);
        /// <summary>
        /// Обновление данных о заказе
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<BaseResult<OrderDto>> UpdateOrderAsync(UpdateOrderDto dto);
    }
}
