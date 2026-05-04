using Microsoft.EntityFrameworkCore;
using Synapse.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Synapse.Application.Common.Behaviours;
using Synapse.Application.Entities.Interfaces;
using Synapse.Application.Features.Auth.Interfaces;
using Synapse.Infrastructure.Auth;
using Synapse.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "../.env"));

builder.Configuration.AddEnvironmentVariables();

var connectionString = $"Server={DotNetEnv.Env.GetString("DB_HOST")};" +
                       $"Port={DotNetEnv.Env.GetString("DB_PORT")};" +
                       $"Database={DotNetEnv.Env.GetString("DB_NAME")};" +
                       $"User Id={DotNetEnv.Env.GetString("DB_USER")};" +
                       $"Password={DotNetEnv.Env.GetString("DB_PASSWORD")};";

builder.Services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(connectionString); });

builder.Services.AddControllers();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
    cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
});

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IPasswordProvider, PasswordProvider>();
builder.Services.AddScoped<IJwtProvider, JWTProvider>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // swagger if needed
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var port = DotNetEnv.Env.GetString("APP_PORT");

app.MapGet("/", () => "Synapse Orders API running.");
app.Run($"http://0.0.0.0:{port}");