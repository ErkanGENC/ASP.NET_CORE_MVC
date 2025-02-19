using Microsoft.AspNetCore.Builder;
using Repositories;

namespace StoreApp.Extensions
{
    public static class ApplicationExtensions
    {
        public static void ConfigureAndSeedDb(this IApplicationBuilder app)
        {
            RepositoryManager.SeedDatabase(app);
        }
    }
} 