using Application.Abstractions.Authentication;
using Application.Abstractions.Caching;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Common.Interfaces;
using Application.Services.Catalog;
using Domain.Abstractions.Security;
using Domain.Services.Catalog;
using Domain.Services.Identity;
using Infrastructure.Caching;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Security;
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
            //DbContext:
            if (env.IsDevelopment())
            {
                services.AddDbContext<AppDbContext>(options =>
                   options.UseInMemoryDatabase("DebugDatabase"));
                //services.AddDbContext<AppDbContext>(option =>
                //   option.UseSqlServer(Environment.GetEnvironmentVariable("Gold_Connection", EnvironmentVariableTarget.Machine)));

            }
            else
            {
                services.AddDbContext<AppDbContext>(option =>
                   option.UseSqlServer(Environment.GetEnvironmentVariable("Gold_Connection", EnvironmentVariableTarget.Machine)));
            }

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

            // Repository:
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOtpCodeRepository, OtpCodeRepository>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<ITokenHasher, TokenHasher>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ICoinRepository, CoinRepository>();
            services.AddScoped<IMarketPriceRepository, MarketPriceRepository>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IMeltedGoldRepository, MeltedGoldRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<RefreshTokenDomainService>();

            // External Services:
            services.AddScoped<ISMSService, SMSService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IMarketPriceService, MarketPriceService>();
            services.AddScoped<IPricingService, PricingService>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<PricingEngine>();
            services.AddHostedService<MarketPriceWorker>();

            return services;
        }
    }
}
