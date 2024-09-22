using Booking.Application.Validations;
using Booking.DAL.Interceptors;
using Booking.DAL.Repositories;
using Booking.Domain.Entity;
using Booking.Domain.Interfaces.Databases;
using Booking.Domain.Interfaces.Repositories;
using Booking.Domain.Interfaces.Validations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DAL.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString  = configuration.GetConnectionString("PostgreSQL");
            services.AddSingleton<DateInterceptor>();
            services.AddDbContext<ApplicationDbContext>(options => 
            { 
                options.UseNpgsql(connectionString); 
              
            });
            services.InitRepositories();
        }

        private static void InitRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            services.AddScoped<IBaseRepository<Facility>, BaseRepository<Facility>>();
            services.AddScoped<IBaseRepository<UserRole>, BaseRepository<UserRole>>();
            services.AddScoped<IBaseRepository<UserToken>, BaseRepository<UserToken>>();
            services.AddScoped<IBaseRepository<Order>, BaseRepository<Order>>();
            services.AddScoped<IBaseRepository<OrderFacility>, BaseRepository<OrderFacility>>();
            services.AddScoped<IBaseRepository<Role>, BaseRepository<Role>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
