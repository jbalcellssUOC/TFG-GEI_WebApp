using DataAccessLayer.Classes;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer.Classes
{
    /// <summary>
    /// Bussiness Logic Service Collection
    /// </summary>
    public static class BLServiceCollection
    {
        /// <summary>
        /// Get service collection from IServiceCollection    
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection GetServiceCollection(IServiceCollection? services = null)
        {
            services ??= new ServiceCollection();

            //SERVICES 
            services.AddBLInjectionExtensions();

            //REPOSITORIES
            services.AddDALInjectionExtensions();

            return services;
        }
    }
}
