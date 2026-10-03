using API.Configuration;
using API.Extensions;
using API.Filters;
using API.Middleware;
using Application;
using Application.Abstractions.Services;
using Application.Common.Mapping;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Configuration;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGoldTS", policy =>
    {
        policy
             .WithOrigins(
                "http://localhost:3000"
             )
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddCustomApiBehavior();

builder.Services.AddHealthChecks();

builder.Services.AddHealthChecksUI(setup =>
{
    setup.AddHealthCheckEndpoint("GoldAPI", "/health");
    setup.SetEvaluationTimeInSeconds(10);

    setup.MaximumHistoryEntriesPerEndpoint(60);
    setup.SetHeaderText("GoldAPI");
})
.AddInMemoryStorage();

var goldApiBaseUrl = builder.Configuration["BrsApi:BaseUrl"]
    ?? "https://api.brsapi.ir/Market/Gold_Currency.php";
var goldApiKey = builder.Configuration["BrsApi:Key"];
var goldApiUrl = string.IsNullOrWhiteSpace(goldApiKey)
    ? goldApiBaseUrl
    : $"{goldApiBaseUrl}?key={Uri.EscapeDataString(goldApiKey)}";

builder.Services.AddHttpClient<IGoldPriceApiClient, GoldPriceApiClient>(client =>
{
    client.BaseAddress = new Uri(goldApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
});


// JWT Key from config
var authSection = builder.Configuration.GetSection("Auth");
var jwtSecret = authSection["JwtSecret"];
builder.Services.Configure<AuthSetting>(authSection);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer = true,
        ValidIssuer = authSection["Issuer"],
        ValidateAudience = true,
        ValidAudience = authSection["Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorizationPolicies();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ResultFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "GoldAPI",
        Version = "v1",
        Description = "سیستم خرید طلای آبشده و سکه"
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"

                }
            },
            Array.Empty<string>()
        }
    });
});


MapsterConfig.Configure();

var app = builder.Build();

app.UseCors("AllowGoldTS");

// Middlewares
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoldAPI v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy] = StatusCodes.Status200OK,
        [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded] = StatusCodes.Status200OK,
        [Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false
});

app.MapHealthChecks("/health/details", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false
});

app.MapHealthChecksUI(setup =>
{
    setup.UIPath = "/health-ui";

    setup.ApiPath = "/health-ui-api";

    setup.WebhookPath = "/health-ui-webhooks";

    //setup.AddCustomStylesheet("/css/healthchecks-custom.css");
});

app.Run();
