using Booking.Domain.Dto.Facility;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Validations.FluentValidations.Facility
{
    public class CreateFacilityValidator : AbstractValidator<CreateFacilityDto>
    {
        public CreateFacilityValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.Cost).NotEmpty();
            RuleFor(x => x.Coefficient).NotEmpty();
        }
    }
}
