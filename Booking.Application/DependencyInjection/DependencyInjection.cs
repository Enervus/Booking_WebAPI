using Booking.Application.Mapping;
using Booking.Application.Services;
using Booking.Application.Validations.FluentValidations.Facility;
using Booking.Application.Validations.FluentValidations.Order;
using Booking.Domain.Dto.Facility;
using Booking.Domain.Interfaces.Services;
using Booking.Domain.Interfaces.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(FacilityMapping));
        InitServices(services);
    }
    public static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IFacilityValidator,FacilityValidator>();
        services.AddScoped<IValidator<CreateFacilityDto>, CreateFacilityValidator>();
        services.AddScoped<IValidator<UpdateFacilityDto>, UpdateFacilityValidator>();

        services.AddScoped<IOrderValidator, OrderValidator>();

        services.AddScoped<IFacilityService, FacilityService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();//для каждого запрос один сервис и, если сервис использвуется, его не нужно для каждого обращения пересоздавать(Scoped)
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
    }
}