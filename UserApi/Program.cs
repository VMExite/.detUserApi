using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using Scalar.AspNetCore;
using UserApi.Data;
using UserApi.Extensions;
using UserApi.Repositories;
using UserApi.Repositories.Impl;
using UserApi.Services;
using NLog.Web;

var logger = LogManager.GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

// logging settings
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();


    builder.Services.AddControllers();
    builder.Services.AddDatabase(builder.Configuration);

    builder.Services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<UserService>();

    var app = builder.Build();

//Use Migrations
    await using (var scope = app.Services.CreateAsyncScope())
    {
        var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        await database.Database.MigrateAsync();
    }

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
