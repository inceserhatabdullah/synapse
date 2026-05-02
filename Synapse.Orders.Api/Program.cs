using System.Text;
using Microsoft.EntityFrameworkCore;
using Synapse.Infrastructure.Persistence;
using Synapse.Domain.Orders;
using Synapse.Orders.Api.endpoints;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Synapse.Infrastructure.Services;
using Synapse.Orders.Api.validators;

DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "../.env"));
DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

var builder = WebApplication.CreateBuilder(args);

var dbUser = DotNetEnv.Env.GetString("DB_USER");
var dbPassword = DotNetEnv.Env.GetString("DB_PASSWORD");
var dbName = DotNetEnv.Env.GetString("DB_NAME");
var dbPort = DotNetEnv.Env.GetString("DB_PORT");
var dbHost = DotNetEnv.Env.GetString("DB_HOST");
var port = DotNetEnv.Env.GetString("APP_PORT") ?? "8080";

var connectionString = $"Server={dbHost};Port={dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};";

builder.Services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(connectionString); });

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = DotNetEnv.Env.GetString("JWT_ISSUER"),
            ValidAudience = DotNetEnv.Env.GetString("JWT_AUDIENCE"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DotNetEnv.Env.GetString("JWT_KEY"))),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

var apiGroup = app.MapGroup("api/v1");

new OrderEndpoints().MapEndpoints(apiGroup);
new AuthEndpoint().MapEndpoints(apiGroup);

app.MapGet("/", () => "Synapse Orders API running.");
app.Run($"http://0.0.0.0:{port}");