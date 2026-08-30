using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Application.Common.Settings;
using ReenbitBooking.Infrastructure.Context;
using ReenbitBooking.Infrastructure.Data;
using ReenbitBooking.Infrastructure.Queries.Rooms;
using ReenbitBooking.Infrastructure.Security;

namespace ReenbitBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        
        services.AddTransient<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddTransient<IRoomQueries, RoomQueries>();

        return services;
    }
}