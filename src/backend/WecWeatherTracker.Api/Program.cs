using WecWeatherTracker.Application;
using WecWeatherTracker.Application.Hubs;
using WecWeatherTracker.Infrastructure;
using WecWeatherTracker.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// ── Infraestrutura (MongoDB, Redis, repositórios, serviço Open-Meteo) ─────────
builder.Services.AddInfrastructure(builder.Configuration);

// ── Application (use cases, background worker de polling) ────────────────────
builder.Services.AddApplication();

// ── Controllers ───────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── SignalR com Redis Backplane ───────────────────────────────────────────────
// O backplane garante que broadcasts funcionem mesmo com múltiplas instâncias da API
var redisConnection = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
builder.Services.AddSignalR()
    .AddStackExchangeRedis(redisConnection, options =>
    {
        options.Configuration.ChannelPrefix = StackExchange.Redis.RedisChannel.Literal("wec-signalr");
    });

// ── CORS ──────────────────────────────────────────────────────────────────────
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? ["http://localhost:8080", "http://localhost:5173"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("WecCors", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Obrigatório para SignalR com WebSockets
    });
});

// ── OpenAPI (Swagger) ─────────────────────────────────────────────────────────
builder.Services.AddOpenApi();

var app = builder.Build();

// ── Seed dos circuitos WEC na inicialização ───────────────────────────────────
await using (var scope = app.Services.CreateAsyncScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<CircuitSeeder>();
    await seeder.SeedAsync();
}

// ── Pipeline HTTP ─────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("WecCors");

app.MapControllers();
app.MapHub<WeatherHub>("/hubs/weather");

app.Run();
