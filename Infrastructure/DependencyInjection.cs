using Application.Abstractions.Authentication;
using Application.Abstractions.Caching;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Common.Interfaces;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Caching;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.Time;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            // DbContext: 
            //if (env.IsDevelopment())
            //{
                //services.AddDbContext<AppDbContext>(options =>
                //    options.UseInMemoryDatabase("DebugDatabase"));

                services.AddDbContext<AppDbContext>(option =>
                    option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //}
            //else
            //{
            //    services.AddDbContext<AppDbContext>(option =>
            //        option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //}

            //Caching:
            services.AddMemoryCache();
            services.AddScoped<ICacheService, MemoryCacheService>();
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = configuration["Redis:ConnectionString"];
            //});
            //services.AddScoped<ICacheService, RedisCacheService>();

            services.AddHttpContextAccessor();

            // Identity:
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            // Repository:
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOtpCodeRepository, OtpCodeRepository>();
            //services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // External Services:
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISMSService, SMSService>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}
