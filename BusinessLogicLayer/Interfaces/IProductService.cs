using Entities.DTOs;
using Entities.Models;

namespace BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for Service for product operations
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Get all products from the database.
        /// </summary>
        /// <returns>List<AppProduct></returns>
        List<AppProduct> GetAllProducts(string username);

        /// <summary>
        /// Get products method
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>PaginatedProductList<AppProduct></returns>
        public Task<PaginatedProductList<AppProduct>> GetProducts(string username, int pageIndex, int pageSize);


        /// <summary>
        /// Add a product to the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>string</returns>
        Task<string> AddProduct(string username, BodyProductDTO bodyProductDTO);

        /// <summary>
        /// Update a product in the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>bool</returns>
        Task<bool> UpdateProduct(string username, Guid productId, BodyProductDTO bodyProductDTO);

        /// <summary>
        /// Delete product method
        /// </summary>
        /// <param name="username"></param>
        /// <param name="product"></param>
        /// <returns>bool</returns>
        public Task<bool> DeleteProductById(string username, Guid productGuid);
    }
}
