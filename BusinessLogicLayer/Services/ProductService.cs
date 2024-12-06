using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using NLog;
using System.Diagnostics;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// ProductService class
    /// </summary>
    /// <param name="ProductRepository"></param>
    /// <param name="UserRepository"></param>
    /// <param name="Mapper"></param>
    public class ProductService(
        IProductRepository ProductRepository,
        IUserRepository UserRepository,
        IMapper Mapper
        ) : IProductService
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get all products method  
        /// </summary>
        /// <returns>List<AppProduct></returns>
        public List<AppProduct> GetAllProducts(string username)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            var products = ProductRepository.GetAllProducts(userGuid);
            return products;
        }

        /// <summary>
        /// Get products method
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>PaginatedProductList<AppProduct></returns>
        public async Task<PaginatedProductList<AppProduct>> GetProducts(string username, int pageIndex, int pageSize)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            var products = await ProductRepository.GetProducts(userGuid, pageIndex, pageSize);
            return products;
        }

        /// <summary>
        /// Add product method
        /// </summary>
        /// <param name="username"></param>
        /// <param name="bodyProductDTO"></param>
        /// <returns>string</returns>
        public async Task<string> AddProduct(string username, BodyProductDTO bodyProductDTO)
        {
            string result = null!;

            try
            {
                var userGuid = UserRepository.GetUserIdByEmail(username);
                var newRow = Mapper.Map<AppProduct>(bodyProductDTO);     // Map from DTO to POCO
                newRow.UserId = userGuid;
                result = await ProductRepository.AddProduct(newRow); 
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return result;
        }

        /// <summary>
        /// Update product method
        /// </summary>
        /// <param name="product"></param>
        /// <returns>string</returns>
        public async Task<bool> UpdateProduct(string username, Guid productId, BodyProductDTO bodyProductDTO)
        {
            bool result = false;
            try
            {
                var userGuid = UserRepository.GetUserIdByEmail(username);
                var product = Mapper.Map<AppProduct>(bodyProductDTO);                   // Map from DTO to POCO
                result = await ProductRepository.UpdateProduct(userGuid, productId, product);
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
            }

            return result;
        }

        /// <summary>
        /// Delete product method
        /// </summary>
        /// <param name="username"></param>
        /// <param name="product"></param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteProductById(string username, Guid productGuid)
        {
            bool result = false;
            try
            {
                var userGuid = UserRepository.GetUserIdByEmail(username);
                result = await ProductRepository.DeleteProductById(userGuid, productGuid);
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
            }

            return result;
        }
    }
}
