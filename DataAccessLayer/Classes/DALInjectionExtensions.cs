using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Classes
{
    /// <summary>
    /// DAL Injection Extensions
    /// </summary>
    public static class DALInjectionExtensions
    {
        /// <summary>
        /// Add DAL Injection Extensions
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDALInjectionExtensions(this IServiceCollection services)
        {
            // REPOSITORIES
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IBarcodeRepository, BarcodeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
