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
        builder.Services.AddSingleton<IGameSessionService, GameSessionService>();
        builder.Services.AddScoped<IGameManagerService, GameManagerService>();
        builder.Services.AddTransient<GameStateDtoMapper>();
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
    }

    public static void ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}