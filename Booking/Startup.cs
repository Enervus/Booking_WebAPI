using Booking.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Xml.Serialization;

namespace Booking.Api
{
    public static class Startup
    {
        /// <summary>
        /// Подключение Swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Введите JWT токен авторизации",
                    Name = "Авторизация",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme ="Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                               Reference = new OpenApiReference()
                               {
                                   Type = ReferenceType.SecurityScheme,
                                   Id = "Bearer"
                               },
                        },
                        new List<string>()
                    }
                });
                var basePath = AppContext.BaseDirectory;

                var xmlPath = Path.Combine(basePath,"BookingAPI.xml");
                options.IncludeXmlComments(xmlPath);
            });
            services.ConfigureOptions<ConfigureSwaggerOptions>();//регистрация настроек
        }

        /// <summary>
        /// Подключение аутентификации и авторизации
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthenticationAndAuthorization(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddAuthorization();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var options = builder.Configuration.GetSection(JwtSettings.DefaultSection).Get<JwtSettings>();
                var jwtKey = options.JwtKey;
                var issuer = options.Issuer;
                var audience = options.Audience;
                o.Authority = options.Authority;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        }

    }
}
