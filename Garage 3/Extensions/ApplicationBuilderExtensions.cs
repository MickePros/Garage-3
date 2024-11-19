using Garage_3.Data;
using Microsoft.EntityFrameworkCore;

namespace Garage_3.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();

                // Uncomment these two for a fresh database installation with SeedData.
                // await context.Database.EnsureDeletedAsync();
                // await context.Database.MigrateAsync();

                try
                {
                    await SeedData.Init(context, services);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return app;
        }
    }
}
