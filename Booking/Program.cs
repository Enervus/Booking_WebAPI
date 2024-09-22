using Booking.DAL.DependencyInjection;
using Booking.Application.DependencyInjection;
using Serilog;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Booking.Api;
using Booking.Domain.Settings;
using Microsoft.AspNetCore.CookiePolicy;
using Booking.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));
builder.Services.AddControllers();

builder.Services.AddAuthenticationAndAuthorization(builder);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwagger();

builder.Host.UseSerilog((context,configuration)=>configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddCors();

var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach(var descriptiom in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"../swagger/{descriptiom.GroupName}/swagger.json/",
                descriptiom.GroupName.ToUpperInvariant());
            //options.RoutePrefix = string.Empty; //для того,чтобы переходить в swagger по https://localhost:44383
        }
    });
}
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});
/*app.Use(async (context, next) =>
{
    var token = context.Request.Cookies[".AspNetCore.Application.Id"];
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer" + token);
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Xss-Protection", "1");
        context.Response.Headers.Add("X-Frame-Options", "DENY");
    }

    await next();
});*/
app.UseCors(x => x.WithOrigins("http://localhost:3000")
.AllowAnyHeader()
.AllowAnyMethod());


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
