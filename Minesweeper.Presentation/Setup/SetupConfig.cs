using Minesweeper.Application.Interfaces;
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