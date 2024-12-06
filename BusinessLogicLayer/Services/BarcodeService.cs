using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.Models;
using ExternalAPI.DTO;
using NLog;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// BarcodeService class
    /// </summary>
    /// <param name="BarcodeRepository"></param>
    public class BarcodeService(
        IBarcodeRepository BarcodeRepository,
        IUserRepository UserRepository
        ) : IBarcodeService
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get all static barcodes of a user
        /// </summary>
        /// <returns>List<AppCBStatic></returns>
        public List<AppCBStatic> GetAllCBStatic(string username)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            return BarcodeRepository.GetAllCBStatic(userGuid);
        }

        /// <summary>
        /// Get all static barcodes of a user
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>PaginatedStaticBarcodeList<AppCBStatic></returns>
        public async Task<PaginatedStaticBarcodeList<AppCBStatic>> GetCBStaticProducts(string username, int pageIndex, int pageSize)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            return await BarcodeRepository.GetCBStaticProducts(userGuid, pageIndex, pageSize);
        }

        /// <summary>
        /// Get all dynamic barcodes of a user
        /// </summary>
        /// <returns>List<AppCBDynamic></returns>
        public List<AppCBDynamic> GetAllCBDynamic(string username)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            return BarcodeRepository.GetAllCBDynamic(userGuid);
        }

        /// <summary>
        /// Get all dynamic barcodes of a user
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PaginatedDynamicBarcodeList<AppCBDynamic>> GetCBDynamicProducts(string username, int pageIndex, int pageSize)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            return await BarcodeRepository.GetCBDynamicProducts(userGuid, pageIndex, pageSize);
        }

        /// <summary>
        /// Get a static barcodes by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns>AppCBStatic</returns>
        public async Task<AppCBStatic> GetCBStaticByCode(string username, string code)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            return await BarcodeRepository.GetCBStaticByCode(userGuid, code);
        }

        /// <summary>
        /// Get a dynamic barcodes by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns>AppCBDynamic</returns>
        public async Task<AppCBDynamic> GetCBDynamicByCode(string username, string code)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            return await BarcodeRepository.GetCBDynamicByCode(userGuid, code);
        }

        /// <summary>
        /// Get a static barcodes by id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>AppCBStatic</returns>
        public async Task<AppCBStatic> GetCBStaticById(string username, Guid barcodeId)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            return await BarcodeRepository.GetCBStaticById(userGuid, barcodeId);
        }

        /// <summary>
        /// Get a dynamic barcodes by id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>AppCBDynamic</returns>
        public async Task<AppCBDynamic> GetCBDynamicById(string username, Guid barcodeId)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            return await BarcodeRepository.GetCBDynamicById(userGuid, barcodeId);
        }

        /// <summary>
        /// Delete a static barcodes by id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteCBStaticById(string username, Guid barcodeId)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            var result = await BarcodeRepository.DeleteCBStaticById(userGuid, barcodeId);
            return result;
        }

        /// <summary>
        /// Delete a dynamic barcodes by id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteCBDynamicById(string username, Guid barcodeId)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            var result = await BarcodeRepository.DeleteCBDynamicById(userGuid, barcodeId);
            return result;
        }

        /// <summary>
        /// Add a static barcode
        /// </summary>
        /// <param name="username"></param>
        /// <param name="bodyBCStaticDTO"></param>
        /// <returns>string</returns>
        public async Task<string> AddCBStatic(string username, BodyBCStaticDTO bodyBCStaticDTO)
        {
            string result = null!;

            try
            {
                var userGuid = UserRepository.GetUserIdByEmail(username);
                AppCBStatic cbStatic = new()
                {
                    UserId = userGuid,
                    Description = bodyBCStaticDTO.Description,
                    CBType = bodyBCStaticDTO.CBType,
                    CBValue = bodyBCStaticDTO.CBValue,
                };
                result = await BarcodeRepository.AddCBStatic(cbStatic);
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return result;
        }

        /// <summary>
        /// Add a dynamic barcode
        /// </summary>
        /// <param name="username"></param>
        /// <param name="bodyBCDynamicDTO"></param>
        /// <returns>string</returns>
        public async Task<string> AddCBDynamic(string username, BodyBCDynamicDTO bodyBCDynamicDTO)
        {
            string result = null!;

            try
            {
                var userGuid = UserRepository.GetUserIdByEmail(username);
                AppCBDynamic cbDynamic = new()
                {
                    UserId = userGuid,
                    Description = bodyBCDynamicDTO.Description,
                    CBType = bodyBCDynamicDTO.CBType,
                    CBValue = bodyBCDynamicDTO.CBValue,
                    CBShortLink = bodyBCDynamicDTO.CBShortLink,
                };
                result = await BarcodeRepository.AddCBDynamic(cbDynamic);
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return result;
        }

        /// <summary>
        /// Update a static barcode
        /// </summary>
        /// <param name="username"></param>
        /// <param name="barcodeId"></param>
        /// <param name="bodyDTO"></param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateCBStatic(string username, Guid barcodeId, BodyBCStaticDTO bodyDTO)
        {
            bool result = false;

            try
            {
                var userGuid = UserRepository.GetUserIdByEmail(username);
                AppCBStatic modBarcode = new()
                {
                    Description = bodyDTO.Description,
                    CBType = bodyDTO.CBType,
                    CBValue = bodyDTO.CBValue,
                };
                result = await BarcodeRepository.UpdateCBStatic(userGuid, barcodeId, modBarcode);

            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return result;
        }

        /// <summary>
        /// Add a dynamic barcode
        /// </summary>
        /// <param name="username"></param>
        /// <param name="barcodeId"></param>
        /// <param name="bodyDTO"></param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateCBDynamic(string username, Guid barcodeId, BodyBCDynamicDTO bodyDTO)
        {
            bool result = false;

            try
            {
                var userGuid = UserRepository.GetUserIdByEmail(username);
                AppCBDynamic modBarcode = new()
                {
                    Description = bodyDTO.Description,
                    CBType = bodyDTO.CBType,
                    CBValue = bodyDTO.CBValue,
                    CBShortLink = bodyDTO.CBShortLink,
                };
                result = await BarcodeRepository.UpdateCBDynamic(userGuid, barcodeId, modBarcode);

            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return result;
        }

        /// <summary>
        /// Delete a dynbamic barcode
        /// </summary>
        /// <param name="cbDynamic"></param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteCBDynamic(string username, AppCBDynamic cbDynamic)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            return await BarcodeRepository.DeleteCBDynamic(userGuid, cbDynamic);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="cbStatic"></param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteCBStatic(string username, AppCBStatic cbStatic)
        {
            var userGuid = UserRepository.GetUserIdByEmail(username);
            return await BarcodeRepository.DeleteCBStatic(userGuid, cbStatic);
        }
    }
}
