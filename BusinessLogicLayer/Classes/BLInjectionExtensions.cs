using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer.Classes
{
    /// <summary>
    /// Business Logic Injection Extensions
    /// </summary>
    public static class BLInjectionExtensions
    {
        /// <summary>
        /// Add Business Logic Injection Extensions with services to the service collection IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBLInjectionExtensions(this IServiceCollection services)
        {
            // SERVICES 
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IHelpersService, EmailBodyHelper>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IPromptService, PromptService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserDDService, UserDDService>();
            services.AddScoped<IBarcodeService, BarcodeService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
