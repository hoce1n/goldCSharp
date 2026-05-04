namespace API.Configuration
{
    public static class AuthorizationPolicies
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomerOnly", policy =>
                    policy.RequireRole("Customer"));

                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("ManagerOnly", policy =>
                    policy.RequireRole("Manager"));

                options.AddPolicy("AdminOrManager", policy =>
                    policy.RequireRole("Admin", "Manager"));

                options.AddPolicy("Authenticated", policy =>
                    policy.RequireAuthenticatedUser());
            });

            return services;
        }
    }
}
