using Microsoft.EntityFrameworkCore;
using Synapse.Infrastructure.Persistence;
using Synapse.Domain.Orders;
using Synapse.Orders.Api.endpoints;

DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "../.env"));

var builder = WebApplication.CreateBuilder(args);

var dbUser = DotNetEnv.Env.GetString("DB_USER");
var dbPassword = DotNetEnv.Env.GetString("DB_PASSWORD");
var dbName = DotNetEnv.Env.GetString("DB_NAME");
var dbPort = DotNetEnv.Env.GetString("DB_PORT");
var dbHost = DotNetEnv.Env.GetString("DB_HOST");
var port = DotNetEnv.Env.GetString("APP_PORT") ?? "8080";

Console.WriteLine($"DB User: {dbUser},  DB Password: {dbPassword}, DB Name: {dbName} , DB Port: {dbPort} , DB Host: {dbHost}");
var connectionString = $"Server={dbHost};Port={dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();
var apiGroup = app.MapGroup("api/v1");

new OrderEndpoints().MapEndpoints(apiGroup);


app.MapGet("/", () => "Synapse Orders API running.");
app.Run($"http://0.0.0.0:{port}");
