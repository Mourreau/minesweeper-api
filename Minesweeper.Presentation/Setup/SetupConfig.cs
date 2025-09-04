using System.Text.Json.Serialization;
using Minesweeper.Application.Interfaces;
using Minesweeper.Application.Mappers;
using Minesweeper.Application.Services;

namespace Minesweeper.Presentation.Setup;

public static class SetupConfig
{
    public static void ConfigureDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        
        var frontOrigin = builder.Configuration["AllowedCorsOrigin"] ?? "http://localhost:5173";
        
        builder.Services.AddSingleton<IGameSessionService, GameSessionService>();
        builder.Services.AddScoped<IGameManagerService, GameManagerService>();
        builder.Services.AddTransient<GameStateDtoMapper>();
        builder.Services.AddAuthorization();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("FrontOnly", policy =>
            {
                policy.WithOrigins(frontOrigin!)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        
        builder.Services.AddControllers()
            .AddJsonOptions(o =>
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    }

    public static void ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("FrontOnly");
        app.UseAuthorization();
        app.MapControllers();
    }
}