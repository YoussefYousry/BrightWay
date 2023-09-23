using BrightWeb_DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using BrightWeb_BAL.Repositories;
using BrightWeb_BAL.Contracts;

namespace BrightWeb.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services,
        IConfiguration configuration)
         => services.AddDbContext<AppDbContext>(opts =>
                                    opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")
                                    , b => b.MigrationsAssembly("BrightWeb")));


        public static void ConfigureLifeTime(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
    }
}
