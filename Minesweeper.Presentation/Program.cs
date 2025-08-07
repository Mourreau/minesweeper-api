using Minesweeper.Presentation.Setup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureMiddleware();

app.Run();