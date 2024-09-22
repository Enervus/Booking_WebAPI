using AutoMapper;
using Booking.Application.Resources;
using Booking.Domain.Dto.Order;
using Booking.Domain.Dto.OrderDto;
using Booking.Domain.Entity;
using Booking.Domain.Enum;
using Booking.Domain.Interfaces.Databases;
using Booking.Domain.Interfaces.Repositories;
using Booking.Domain.Interfaces.Services;
using Booking.Domain.Interfaces.Validations;
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
    /// <inheritdoc/>
    public class OrderService:IOrderService
    {
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Facility> _facilityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrderValidator _orderValidator;

        public OrderService(IBaseRepository<Order> orderRepository, ILogger logger, IMapper mapper, IBaseRepository<User> userRepository, IOrderValidator orderValidator
            , IBaseRepository<Facility> facilityRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _facilityRepository = facilityRepository;
            _logger = logger;
            _mapper = mapper;
            _orderValidator = orderValidator;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc/>
        public async Task<CollectionResult<OrderDto>> GetOrdersAsync(long userId)
        {
            OrderDto[] orders;
            orders = await _orderRepository.GetAll()
                     .Include(x=>x.Facilities)
                     .Where(x => x.UserId == userId)
                     .Select(x => _mapper.Map<OrderDto>(x))
                     .ToArrayAsync();

            if (!orders.Any())
            {
                _logger.Warning(ErrorMessage.OrdersNotFound, orders.Length);
                return new CollectionResult<OrderDto>()
                {
                    ErrorMessage = ErrorMessage.OrdersNotFound,
                    ErrorCode = (int)ErrorCodes.OrdersNotFound
                };
            }

            return new CollectionResult<OrderDto>()
            {
                Data = orders,
                Count = orders.Length
            };
        }

        /// <inheritdoc/>
        public Task<BaseResult<OrderDto>> GetOrderByIdAsync(long id)
        {
            OrderDto? order;
            order = _orderRepository.GetAll()
                                .Include(x=>x.Facilities)
                                .AsEnumerable()
                                .Select(x => _mapper.Map<OrderDto>(x))
                                .FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                _logger.Warning($"Объект с {id} не найден");
                return Task.FromResult(new BaseResult<OrderDto>
                {
                    ErrorMessage = ErrorMessage.OrderNotFound,
                    ErrorCode = (int)ErrorCodes.OrderNotFound
                });
            }

            return Task.FromResult(new BaseResult<OrderDto>
            {
                Data = order
            });
        }

        /// <inheritdoc/>
        public async Task<BaseResult<OrderDto>> CreateOrderAsync(CreateOrderDto dto)
        {
            Order order =  new Order();

            List<Facility> facilities = new List<Facility>();
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == dto.UserId);

            foreach (long id in dto.FacilityIds)
                facilities.Add(await _facilityRepository.GetAll()
               .FirstOrDefaultAsync(x => x.Id == id));

            var result = _orderValidator.CreateValidator(user, facilities);
            if (!result.IsSuccess)
            {
                return new BaseResult<OrderDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode
                };
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    order = new Order()
                    {
                        Facilities = facilities,
                        TotalCost = dto.TotalCost,
                        UserId = user.Id
                    };
                    await _unitOfWork.Orders.CreateAsync(order);
                    await _unitOfWork.SaveChangesAsync();

                    for(int i = 0; i < facilities.Count;i++)
                    { 
                        facilities[i].EndOfLease = DateTime.UtcNow.AddDays(dto.TimeOfLease[i]);
                        facilities[i].Status = Status.Taken;
                        _unitOfWork.Facilities.Update(facilities[i]);
                    }
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();

                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }
            return new BaseResult<OrderDto>()
            {
                Data = _mapper.Map<OrderDto>(order)
            };

        }

        /// <inheritdoc/>
        public async Task<BaseResult<OrderDto>> DeleteOrderAsync(long id)
        {
            var order = await _orderRepository.GetAll()
                                .FirstOrDefaultAsync(x => x.Id == id);
            var result = _orderValidator.ValidateOnNull(order);
            if (!result.IsSuccess)
            {
                return new BaseResult<OrderDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode
                };
            }

            _orderRepository.Remove(order);
            await _orderRepository.SaveChangesAsync();
            return new BaseResult<OrderDto>()
            {
                Data = _mapper.Map<OrderDto>(order)
            };
        }

        /// <inheritdoc/>
        public async Task<BaseResult<OrderDto>> UpdateOrderAsync(UpdateOrderDto dto)
        {
            List<Facility> facilities = new List<Facility>();
            foreach (long id in dto.FacilityIds)
                facilities.Add(await _facilityRepository.GetAll()
               .FirstOrDefaultAsync(x => x.Id == id));

            var order = await _orderRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == dto.Id);
            var result = _orderValidator.ValidateOnNull(order);

            if (!result.IsSuccess)
            {
                return new BaseResult<OrderDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode
                };
            }
            Order updatedOrder = order;
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    order.TotalCost = dto.TotalCost;
                    order.Facilities = facilities;
                    updatedOrder = _unitOfWork.Orders.Update(order);
                    await _unitOfWork.SaveChangesAsync();

                    for (int i = 0; i < facilities.Count; i++)
                    {
                        facilities[i].EndOfLease = DateTime.UtcNow.AddDays(dto.TimeOfLease[i]);
                        _unitOfWork.Facilities.Update(facilities[i]);
                    }
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }
            return new BaseResult<OrderDto>()
            {
                Data = _mapper.Map<OrderDto>(updatedOrder)
            };
        }
    }
}
