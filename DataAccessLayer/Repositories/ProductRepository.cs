using DataAccessLayer.Interfaces;
using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.Diagnostics;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Product repository
    /// </summary>
    /// <param name="bbddcontext"></param>
    public class ProductRepository(BBDDContext bbddcontext) : IProductRepository
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// get paginated all products from the database
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PaginatedProductList<AppProduct>> GetProducts(Guid userGuid, int pageIndex, int pageSize)
        {
            var products = await bbddcontext.AppProducts
                .Where(p => p.UserId == userGuid)
                .OrderBy(b => b.ProductId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await bbddcontext.AppProducts.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedProductList<AppProduct>(products, pageIndex, totalPages);
        }

        /// <summary>
        /// Get all products from the database.
        /// </summary>
        /// <returns></returns>
        public List<AppProduct> GetAllProducts(Guid userGuid)
        {
            var products = bbddcontext.AppProducts
                .Where(p => p.UserId == userGuid)
                .AsNoTracking()
                .ToList();
            return products;
        }

        /// <summary>
        /// Add a product to the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<string> AddProduct(AppProduct product)
        {
            string resultGuid = null!;
            try
            {
                var newProduct = bbddcontext.Add(product);
                await bbddcontext.SaveChangesAsync();
                resultGuid = newProduct.GetDatabaseValues()!.GetValue<Guid>("ProductId").ToString();
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
            }

            return resultGuid;
        }

        /// <summary>
        /// Update a product in the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> UpdateProduct(Guid userGuid, Guid productId, AppProduct product)
        {
            bool result = false;
            try
            {
                var updateProduct = bbddcontext.AppProducts.Where(x => x.UserId == userGuid && x.ProductId == productId).FirstOrDefault();
                if (updateProduct != null)
                {
                    updateProduct.Category = product.Category.IsNullOrEmpty() ? product.Category : product.Category;
                    updateProduct.Description = product.Description.IsNullOrEmpty() ? product.Description : product.Description;
                    updateProduct.Price = product.Price;
                    updateProduct.CBType = product.CBType.IsNullOrEmpty() ? product.CBType : product.CBType;
                    updateProduct.CBValue = product.CBValue.IsNullOrEmpty() ? product.CBValue : product.CBValue;
                    updateProduct.CBShortLink = product.CBShortLink.IsNullOrEmpty() ? product.CBShortLink : product.CBShortLink;
                    updateProduct.IsoDateM = DateTime.Now;

                    bbddcontext.Update(updateProduct);
                    await bbddcontext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
            }

            return result;
        }

        /// <summary>
        /// Delete a product from the database.
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="product"></param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteProductById(Guid userGuid, Guid productGuid)
        {
            bool result = false;
            try
            {
                var rowLocated = bbddcontext.AppProducts
                    .Where(AppProduct => AppProduct.UserId == userGuid && AppProduct.ProductId == productGuid)
                    .FirstOrDefault();
                if (rowLocated != null)
                {
                    bbddcontext.Remove(rowLocated);
                    await bbddcontext.SaveChangesAsync();
                    result = true;
                }
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
