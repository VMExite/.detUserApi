using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Data;

namespace UserApi.Extentions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultDatabase")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultDatabase' was not found.");

        services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
