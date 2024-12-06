using Entities.Models;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Product repository interface
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// get paginated all products from the database
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>PaginatedProductList<AppProduct></returns>
        public Task<PaginatedProductList<AppProduct>> GetProducts(Guid userGuid, int pageIndex, int pageSize);

        /// <summary>
        /// Get all products from the database.
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns>List<AppProduct></returns>
        public List<AppProduct> GetAllProducts(Guid userGuid);

        /// <summary>
        /// Add a product to the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>string</returns>
        public Task<string> AddProduct(AppProduct product);

        /// <summary>
        /// Update a product in the database.
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="ProductId"></param>
        /// <param name="product"></param>
        /// <returns>string</returns>
        public Task<bool> UpdateProduct(Guid userGuid, Guid ProductId, AppProduct product);

        /// <summary>
        /// Delete a product from the database.
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="product"></param>
        /// <returns>bool</returns>
        public Task<bool> DeleteProductById(Guid userGuid, Guid productGuid);
    }
}
