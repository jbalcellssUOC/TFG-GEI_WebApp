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
    /// Barcode repository
    /// </summary>
    /// <param name="bbddcontext"></param>
    public class BarcodeRepository(BBDDContext bbddcontext): IBarcodeRepository
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// get paginated all static barcodes from the database
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PaginatedStaticBarcodeList<AppCBStatic>> GetCBStaticProducts(Guid userGuid, int pageIndex, int pageSize)
        {
            var CBProducts = await bbddcontext.AppCBStatics
                        .Where(AppProduct => AppProduct.UserId == userGuid)
                        .OrderBy(b => b.BarcodeId)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            var count = await bbddcontext.AppCBStatics.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedStaticBarcodeList<AppCBStatic>(CBProducts, pageIndex, totalPages);
        }

        /// <summary>
        /// get paginated all dynamic barcodes from the database
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PaginatedDynamicBarcodeList<AppCBDynamic>> GetCBDynamicProducts(Guid userGuid, int pageIndex, int pageSize)
        {
            var CBProducts = await bbddcontext.AppCBDynamics
                        .Where(AppProduct => AppProduct.UserId == userGuid)
                        .OrderBy(b => b.BarcodeId)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            var count = await bbddcontext.AppCBDynamics.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedDynamicBarcodeList<AppCBDynamic>(CBProducts, pageIndex, totalPages);
        }

        /// <summary>
        /// Get all static barcodes from database
        /// </summary>
        /// <returns></returns>
        public List<AppCBStatic> GetAllCBStatic(Guid userGuid)
        {
            var cbStatic = bbddcontext.AppCBStatics
                .AsNoTracking()
                .ToList();
            return cbStatic;
        }

        /// <summary>
        /// Get all dynamic barcodes from database
        /// </summary>
        /// <returns></returns>
        public List<AppCBDynamic> GetAllCBDynamic(Guid userGuid)
        {
            var cbStatic = bbddcontext.AppCBDynamics
                .Where(AppProduct => AppProduct.UserId == userGuid)
                .AsNoTracking()
                .ToList();
            return cbStatic;
        }

        /// <summary>
        /// Add a static barcode to the database
        /// </summary>
        /// <param name="cbStatic"></param>
        /// <returns></returns>
        public async Task<string> AddCBStatic(AppCBStatic cbStatic)
        {
            string resultGuid = null!;
            try
            {
                var newcb = bbddcontext.Add(cbStatic);
                await bbddcontext.SaveChangesAsync();
                resultGuid = newcb.GetDatabaseValues()!.GetValue<Guid>("BarcodeId").ToString();
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
            }

            return resultGuid;
        }

        /// <summary>
        /// Add a dynamic barcode to the database
        /// </summary>
        /// <param name="cbDynamic"></param>
        /// <returns></returns>
        public async Task<string> AddCBDynamic(AppCBDynamic cbDynamic)
        {
            string resultGuid = null!;
            try
            {
                var newcb = bbddcontext.Add(cbDynamic);
                await bbddcontext.SaveChangesAsync();
                resultGuid = newcb.GetDatabaseValues()!.GetValue<Guid>("BarcodeId").ToString();
            }
            catch (Exception ex)
            {
                if (!Debugger.IsAttached)
                    Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? ""}[StackT]: {ex.StackTrace ?? ""}[HLink]: {ex.HelpLink ?? ""}[HResult]: {ex.HResult}[Source]: {ex.Source ?? ""}{(ex.Data?.Count > 0 ? ex.Data : "")}[InnerE]: {ex.InnerException?.Message ?? ""}");
            }

            return resultGuid;
        }

        /// <summary>
        /// Update a static barcode in the database
        /// </summary>
        /// <param name="cbStatic"></param>
        /// <returns></returns>
        public async Task<bool> UpdateCBStatic(Guid userGuid, Guid barcodeId, AppCBStatic modBarcode)
        {
            bool result = false;
            try
            {
                var updateBarcode = bbddcontext.AppCBStatics.Where(x => x.UserId == userGuid && x.BarcodeId == barcodeId).FirstOrDefault();
                if (updateBarcode != null)
                {
                    updateBarcode.Description = !modBarcode.Description.IsNullOrEmpty() ? modBarcode.Description : updateBarcode.Description;
                    updateBarcode.CBType = !modBarcode.CBType.IsNullOrEmpty() ? modBarcode.CBType : updateBarcode.CBType;
                    updateBarcode.CBValue = !modBarcode.CBValue.IsNullOrEmpty() ? modBarcode.CBValue : updateBarcode.CBValue;

                    updateBarcode.IsoDateM = DateTime.Now;

                    bbddcontext.Update(updateBarcode);
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
        /// Update a dynamic barcode in the database
        /// </summary>
        /// <param name="cbDynamic"></param>
        /// <returns></returns>
        public async Task<bool> UpdateCBDynamic(Guid userGuid, Guid barcodeId, AppCBDynamic modBarcode)
        {
            bool result = false;
            try
            {
                var updateBarcode = bbddcontext.AppCBDynamics.Where(x => x.UserId == userGuid && x.BarcodeId == barcodeId).FirstOrDefault();
                if (updateBarcode != null)
                {
                    updateBarcode.Description = !modBarcode.Description.IsNullOrEmpty() ? modBarcode.Description : updateBarcode.Description;
                    updateBarcode.CBType = !modBarcode.CBType.IsNullOrEmpty() ? modBarcode.CBType : updateBarcode.CBType;
                    updateBarcode.CBValue = !modBarcode.CBValue.IsNullOrEmpty() ? modBarcode.CBValue : updateBarcode.CBValue;
                    updateBarcode.CBShortLink = !modBarcode.CBShortLink.IsNullOrEmpty() ? modBarcode.CBShortLink : updateBarcode.CBShortLink;

                    updateBarcode.IsoDateM = DateTime.Now;

                    bbddcontext.Update(updateBarcode);
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
        /// Delete a static barcode from the database
        /// </summary>
        /// <param name="cbStatic"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCBStatic(Guid userGuid, AppCBStatic cbStatic)
        {
            bool result = false;
            try
            {
                var bclocated = bbddcontext.AppCBStatics
                    .Where(AppCBStatic => AppCBStatic.UserId == userGuid && AppCBStatic.BarcodeId == cbStatic.BarcodeId)
                    .FirstOrDefault();
                if (bclocated != null)
                {
                    bbddcontext.Remove(cbStatic);
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

        /// <summary>
        /// Delete a dynamic barcode from the database
        /// </summary>
        /// <param name="cbDynamic"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCBDynamic(Guid userGuid, AppCBDynamic cbDynamic)
        {
            bool result = false;
            try
            {
                var bclocated = bbddcontext.AppCBDynamics
                    .Where(AppCBDynamic => AppCBDynamic.UserId == userGuid && AppCBDynamic.BarcodeId == cbDynamic.BarcodeId)
                    .FirstOrDefault();
                if (bclocated != null)
                {
                    bbddcontext.Remove(cbDynamic);
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

        /// <summary>
        /// Delete a static barcode by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCBStaticById(Guid userGuid, Guid guid)
        {
            bool result = false;
            try
            {
                var cbStatic = bbddcontext.AppCBStatics
                    .Where(AppProduct => AppProduct.UserId == userGuid && AppProduct.BarcodeId == guid)
                    .FirstOrDefault();
                if (cbStatic != null)
                {
                    bbddcontext.Remove(cbStatic);
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

        /// <summary>
        /// Delete a dynamic barcode by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCBDynamicById(Guid userGuid, Guid guid)
        {
            bool result = false;
            try
            {
                var cbDynamic = bbddcontext.AppCBDynamics
                    .Where(AppProduct => AppProduct.UserId == userGuid && AppProduct.BarcodeId == guid)
                    .FirstOrDefault();
                if (cbDynamic != null)
                {
                    bbddcontext.Remove(cbDynamic);
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

        /// <summary>
        /// Get a static barcode by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AppCBStatic> GetCBStaticById(Guid userGuid, Guid guid)
        {
            var cbStatic = await bbddcontext.AppCBStatics
                .Where(AppProduct => AppProduct.UserId == userGuid && AppProduct.BarcodeId == guid)
                .FirstOrDefaultAsync();
            return cbStatic!;
        }

        /// <summary>
        /// Get a dynamic barcode by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AppCBDynamic> GetCBDynamicById(Guid userGuid, Guid guid)
        {
            var cbDynamic = await bbddcontext.AppCBDynamics
                .Where(AppProduct => AppProduct.UserId == userGuid && AppProduct.BarcodeId == guid)
                .FirstOrDefaultAsync();
            return cbDynamic!;
        }

        /// <summary>
        /// Get a static barcode by its code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<AppCBStatic> GetCBStaticByCode(Guid userGuid, string code)
        {
            var cbStatic = await bbddcontext.AppCBStatics
                .Where(AppProduct => AppProduct.UserId == userGuid && AppProduct.CBValue == code)
                .FirstOrDefaultAsync();
            return cbStatic!;
        }

        /// <summary>
        /// Get a dynamic barcode by its code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<AppCBDynamic> GetCBDynamicByCode(Guid userGuid, string code)
        {
            var cbDynamic = await bbddcontext.AppCBDynamics
                .Where(AppProduct => AppProduct.UserId == userGuid && AppProduct.CBValue == code)
                .FirstOrDefaultAsync();
            return cbDynamic!;
        }
    }
}
