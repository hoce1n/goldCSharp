//using Domain.Entities.Identity;
//using Infrastructure.Persistence.Data;

//namespace API.Extensions
//{
//    public static class DatabaseSeeder
//    {
//        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
//        {
//            using var scope = serviceProvider.CreateScope();
//            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//            if (!context.Roles.Any())
//            {
//                var roles = new[]
//                {
//                    new Role("Customer"),
//                    new Role("Admin"),
//                    new Role("Manager")
//                };

//                await context.Roles.AddRangeAsync(roles);
//                await context.SaveChangesAsync();
//            }
//        }
//    }
//}
