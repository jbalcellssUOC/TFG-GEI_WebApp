using Entities.Models;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Barcode repository interface
    /// </summary>
    public interface IBarcodeRepository
    {
        /// <summary>
        /// get paginated all static barcodes from the database
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>PaginatedStaticBarcodeList<AppCBStatic></returns>
        public Task<PaginatedStaticBarcodeList<AppCBStatic>> GetCBStaticProducts(Guid userGuid, int pageIndex, int pageSize);

        /// <summary>
        /// get paginated all dynamic barcodes from the database
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Task<PaginatedDynamicBarcodeList<AppCBDynamic>> GetCBDynamicProducts(Guid userGuid, int pageIndex, int pageSize);

        /// <summary>
        /// Get all static barcodes from database
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns>List<AppCBStatic></returns>
        public List<AppCBStatic> GetAllCBStatic(Guid userGuid);

        /// <summary>
        /// Get all dynamic barcodes from database
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns>List<AppCBDynamic></returns>
        public List<AppCBDynamic> GetAllCBDynamic(Guid userGuid);

        /// <summary>
        /// Get a dynamic barcode by its code
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="code"></param>
        /// <returns>AppCBDynamic</returns>
        Task<AppCBDynamic> GetCBDynamicByCode(Guid userGuid, string code);

        /// <summary>
        /// Get a static barcode by its code
        /// </summary>
        /// <param name="userGuid"></param> 
        /// <param name="code"></param>
        /// <returns>AppCBStatic</returns>
        Task<AppCBStatic> GetCBStaticByCode(Guid userGuid, string code);

        /// <summary>
        /// Get a dynamic barcode by its id
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="id"></param>
        /// <returns>AppCBDynamic</returns>
        Task<AppCBDynamic> GetCBDynamicById(Guid userGuid, Guid guid);

        /// <summary>
        /// Get a static barcode by its id
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="id"></param>
        /// <returns>AppCBStatic</returns>
        Task<AppCBStatic> GetCBStaticById(Guid userGuid, Guid guid);

        /// <summary>
        /// Delete a dynamic barcode by its id
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        Task<bool> DeleteCBDynamicById(Guid userGuid, Guid guid);

        /// <summary>
        /// Delete a static barcode by its id
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        Task<bool> DeleteCBStaticById(Guid userGuid, Guid guid);

        /// <summary>
        /// Delete a dynamic barcode
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="cbDynamic"></param>
        /// <returns>bool</returns>
        Task<bool> DeleteCBDynamic(Guid userGuid, AppCBDynamic cbDynamic);

        /// <summary>
        /// Delete a static barcode
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="cbStatic"></param>
        /// <returns>bool</returns>
        Task<bool> DeleteCBStatic(Guid userGuid, AppCBStatic cbStatic);

        /// <summary>
        /// Update a dynamic barcode
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="barcodeId"></param>
        /// <param name="modBarcode"></param>
        /// <returns>bool</returns>
        Task<bool> UpdateCBDynamic(Guid userGuid, Guid barcodeId, AppCBDynamic modBarcode);

        /// <summary>
        /// Update a static barcode    
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="barcodeId"></param>
        /// <param name="modBarcode"></param>
        /// <returns>bool</returns>
        Task<bool> UpdateCBStatic(Guid userGuid, Guid barcodeId, AppCBStatic modBarcode);

        /// <summary>
        /// Add a dynamic barcode
        /// </summary>
        /// <param name="cbDynamic"></param>
        /// <returns>string</returns>
        Task<string> AddCBDynamic(AppCBDynamic cbDynamic);

        /// <summary>
        /// Add a static barcode
        /// </summary>
        /// <param name="cbStatic"></param>
        /// <returns>string</returns>
        Task<string> AddCBStatic(AppCBStatic cbStatic);
    }
}
