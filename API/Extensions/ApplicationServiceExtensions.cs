using System.Text;
using API.Data;
using API.Interfaces;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
    {
        
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();
        services.AddScoped<UserRepository>();
        services.AddScoped<AccountRepository>();
        services.AddScoped<ITokenService, TokenService>();  //Am putea include si doar TokenService, nu am inteles diferenta

        return services;
    }
}