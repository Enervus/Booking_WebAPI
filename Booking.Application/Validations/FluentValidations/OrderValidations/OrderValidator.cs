
using Booking.Application.Resources;
using Booking.Domain.Entity;
using Booking.Domain.Enum;
using Booking.Domain.Interfaces.Validations;
using Booking.Domain.Result;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Validations.FluentValidations.Order
{
    public class OrderValidator : IOrderValidator
    {
        public BaseResult CreateValidator(User user, List<Domain.Entity.Facility> facilities)
        {
            if(user == null)
            {
                return new BaseResult()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }

            if (!facilities.Any())
            {
                return new BaseResult()
                {
                    ErrorMessage = ErrorMessage.FacilitiesNotFound,
                    ErrorCode = (int)ErrorCodes.FacilitiesNotFound
                };
            }
            return new BaseResult();
        }

        public BaseResult ValidateOnNull(Domain.Entity.Order model)
        {
             if(model == null)
            {
                return new BaseResult()
                {
                    ErrorMessage = ErrorMessage.OrderNotFound,
                    ErrorCode = (int)ErrorCodes.OrderNotFound
                };
            }
            return new BaseResult();
        }
    }
}
