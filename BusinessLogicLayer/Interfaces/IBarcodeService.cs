using Entities.Models;
using ExternalAPI.DTO;

namespace BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Interface for Service for barcode operations
    /// </summary>
    public interface IBarcodeService
    {
        /// <summary>
        /// Get all static barcodes from database
        /// </summary>
        /// <returns>List<AppCBStatic></returns>
        public List<AppCBStatic> GetAllCBStatic(string username);

        /// <summary>
        /// Get paginated all static barcodes from the database
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Task<PaginatedStaticBarcodeList<AppCBStatic>> GetCBStaticProducts(string username, int pageIndex, int pageSize);

        /// <summary>
        /// Get all dynamic barcodes from database
        /// </summary>
        /// <returns>List<AppCBDynamic></returns>
        public List<AppCBDynamic> GetAllCBDynamic(string username);

        /// <summary>
        /// Get paginated all dynamic barcodes from the database
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Task<PaginatedDynamicBarcodeList<AppCBDynamic>> GetCBDynamicProducts(string username, int pageIndex, int pageSize);

        /// <summary>
        /// Get a dynamic barcode by its code
        /// </summary>
        /// <param name="code"></param>
        /// <returns>AppCBDynamic</returns>
        public Task<AppCBDynamic> GetCBDynamicByCode(string username, string code);

        /// <summary>
        /// Get a static barcode by its code
        /// </summary>
        /// <param name="code"></param>
        /// <returns>AppCBStatic</returns>
        public Task<AppCBStatic> GetCBStaticByCode(string username, string code);

        /// <summary>
        /// Get a dynamic barcode by its id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>AppCBDynamic</returns>
        public Task<AppCBDynamic> GetCBDynamicById(string username, Guid barcodeId);

        /// <summary>
        /// Get a static barcode by its id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>AppCBStatic</returns>
        public Task<AppCBStatic> GetCBStaticById(string username, Guid barcodeId);

        /// <summary>
        /// Delete a dynamic barcode by its id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>bool</returns>
        public Task<bool> DeleteCBDynamicById(string username, Guid barcodeId);

        /// <summary>
        /// Delete a static barcode by its id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>bool</returns>
        public Task<bool> DeleteCBStaticById(string username, Guid barcodeId);

        /// <summary>
        /// Delete a dynamic barcode
        /// </summary>
        /// <param name="cbDynamic"></param>
        /// <returns>bool</returns>
        public Task<bool> DeleteCBDynamic(string username, AppCBDynamic cbDynamic);

        /// <summary>
        /// Delete a static barcode
        /// </summary>
        /// <param name="cbStatic"></param>
        /// <returns>bool</returns>
        public Task<bool> DeleteCBStatic(string username, AppCBStatic cbStatic);

        /// <summary>
        /// Update a dynamic barcode
        /// </summary>
        /// <param name="username"></param>
        /// <param name="barcodeId"></param>
        /// <param name="bodyDTO"></param>
        /// <returns>bool</returns>
        public Task<bool> UpdateCBDynamic(string username, Guid barcodeId, BodyBCDynamicDTO bodyDTO);

        /// <summary>
        /// Update a static barcode    
        /// </summary>
        /// <param name="username"></param>
        /// <param name="barcodeId"></param>
        /// <param name="bodyDTO"></param>
        /// <returns>bool</returns>
        public Task<bool> UpdateCBStatic(string username, Guid barcodeId, BodyBCStaticDTO bodyDTO);

        /// <summary>
        /// Add a dynamic barcode
        /// </summary>
        /// <param name="username"></param>
        /// <param name="bodyBCDynamicDTO"></param>
        /// <returns>string</returns>
        public Task<string> AddCBDynamic(string username, BodyBCDynamicDTO bodyBCDynamicDTO);

        /// <summary>
        /// Add a static barcode
        /// </summary>
        /// <param name="username"></param>
        /// <param name="bodyBCStaticDTO"></param>
        /// <returns>string</returns>
        public Task<string> AddCBStatic(string username, BodyBCStaticDTO bodyBCStaticDTO);
    }
}
