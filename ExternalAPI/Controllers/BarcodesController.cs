using Asp.Versioning;
using AutoMapper;
using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using Entities.POCO;
using ExternalAPI.DTO;
using ExternalAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Swashbuckle.AspNetCore.Annotations;

namespace ExternalAPI.Controllers
{
    /// <summary>
    /// Barcodes Controller class
    /// </summary>
    /// <param name="BarcodeService"></param>
    /// <param name="Mapper"></param>
    /// <param name="SystemUtilsHelper"></param>
    [ApiVersion(1.0)]
    [ApiController]
    [Route("v1/[controller]")]
    public class BarcodesController(
        IBarcodeService BarcodeService,
        IMapper Mapper,
        ISystemUtilsHelper SystemUtilsHelper
        ) : ControllerBase
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /******************************************************************************/
        /**************** PRIVATE METHODS (FOR AUTHENTICATED USERS) *******************/
        /******************************************************************************/

        #region **Static Barcodes**

        /// <summary>
        /// Retrieve all static barcodes.
        /// </summary>
        /// <param name="pageIndex">The index of the page to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of items per page. Defaults to 10.</param>
        /// <response code="200">Succesfully. Returns an object ApiResponseStaticBarcodes with a list of all user static barcodes.</response>
        /// <response code="400">Bad request. Error in the request parameters.</response>
        /// <response code="401">Unauthorized. You are not authorized to use this method.</response>
        /// <response code="405">Method Not Allowed.</response>
        /// <response code="422">Unprocessable Entity. Unexpected API error.</response>
        /// <response code="429">Too Many Requests. The application has exceeded the API rate limit.</response>
        [SwaggerOperation(Description = "This endpoint allows clients to fetch a comprehensive list of all static barcodes created by a Codis365 user. Static barcodes are pre-generated barcodes that do not change and can be used for various purposes such as product identification, tracking, and more. This endpoint provides an efficient way to access all static barcode information in one request. <br/><br/>" +
          "The result will be a JSON list with the available static barcodes and their attributes.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = ["Codis365 Static Barcodes"])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseStaticBarcodes))]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpGet("getstaticbarcodes")]
        [Authorize]
        public async Task<ActionResult<ApiResponseStaticBarcodes>> GetStaticBarcodes(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var username = SystemUtilsHelper.GetUsername();
                var barcodes = await BarcodeService.GetCBStaticProducts(username, pageIndex, pageSize);           // Get all static barcodes
                var barcodesDTO = Mapper.Map<PaginatedStaticBarcodeList<ApiBCStaticDTO>>(barcodes);     // Map to DTO
                var response = new ApiResponseStaticBarcodes(true, "All user static barcodes have been retrieved succesfully.", barcodesDTO);
                return Ok(response);
            }
            catch (Exception Ex)
            {
                var errorResponse = new ApiResponseStaticBarcodes(false, Ex.Message, null!);            // Create new api response    
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);                      // Return error response
            }
        }

        /// <summary>
        /// Add new static barcode.
        /// </summary>
        /// <param name="bodyBCStaticDTO">The data transfer object containing the details of the static barcode to add.</param>
        /// <returns>A response indicating the result of the operation.</returns>        
        [SwaggerOperation(Description = "This endpoint allows advanced customers to create new fixed barcodes for them. These static barcodes remain constant once created and can be used for various purposes such as product identification, inventory tracking, and sales processing. This functionality is essential for maintaining a consistent and reliable product catalog.<br/><br/>" +
          "The response will be a JSON object containing the status of the operation and the unique ID of the new generated product.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = ["Codis365 Static Barcodes"])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiGenericResponse))]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpPost("addstaticbarcode")]
        [Authorize]
        public async Task<ActionResult<ApiGenericResponse>> AddStaticBarcode([FromForm] BodyBCStaticDTO bodyBCStaticDTO)
        {
            ApiGenericResponse apiGenericResponse = new();
            try
            {
                var username = SystemUtilsHelper.GetUsername();
                var result = await BarcodeService.AddCBStatic(username, bodyBCStaticDTO);

                if (string.IsNullOrEmpty(result))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, null);
                }
                else
                {
                    apiGenericResponse.Message = "The new static barcode was created successfully.";
                    apiGenericResponse.Data = result;

                    return StatusCode(StatusCodes.Status200OK, apiGenericResponse);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, null);
        }

        /// <summary>
        /// Delete static barcode.
        /// </summary>
        /// <param name="barcodeId">The ID of the static barcode to delete.</param>
        /// <returns>A status message indicating the result of the operation.</returns>
        [SwaggerOperation(Description = "This endpoint allows clients to delete products. <br/><br/>" +
          "The response will be a JSON object containing the status of the operation and the unique ID of the deleted product.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = ["Codis365 Static Barcodes"])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiGenericResponse))]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpDelete("deletestaticbarcode/{barcodeId}")]
        [Authorize]
        public async Task<ActionResult<ApiGenericResponse>> DeleteStaticBarcode(string barcodeId)
        {
            ApiGenericResponse apiGenericResponse = new();
            try
            {
                bool v = Guid.TryParse(barcodeId, out Guid barcodeGuid);
                var username = SystemUtilsHelper.GetUsername();
                var result = await BarcodeService.DeleteCBStaticById(username, barcodeGuid);

                if (result)
                {
                    apiGenericResponse.Message = "The static barcode was deleted successfully.";
                    apiGenericResponse.Data = barcodeId;

                    return StatusCode(StatusCodes.Status200OK, apiGenericResponse);
                }
                else
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, null);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, null);
        }

        /// <summary>
        /// Update static barcode data.
        /// </summary>
        /// <param name="barcodeId">The ID of the static barcode to be updated.</param>
        /// <param name="bodyBCStaticDTO">The new data to be updates.</param>
        /// <returns>A status message indicating the result of the operation.</returns>
        [SwaggerOperation(Description = "This endpoint allows clients to update product data.<br/><br/>" +
          "The response will be a JSON object containing the status of the operation and the unique ID of the product.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = ["Codis365 Static Barcodes"])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiGenericResponse))]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpPut("updatestaticbarcode/{barcodeId}")]
        [Authorize]
        public async Task<ActionResult<ApiGenericResponse>> UpdateStaticBarcode(
            [FromRoute] string barcodeId,
            [FromForm] BodyBCStaticDTO bodyBCStaticDTO)
        {
            ApiGenericResponse apiGenericResponse = new();
            try
            {
                bool v = Guid.TryParse(barcodeId, out Guid barcodeGuid);
                var username = SystemUtilsHelper.GetUsername();
                var modBarcode = new BodyBCStaticDTO
                {
                    Description = bodyBCStaticDTO.Description,
                    CBType = bodyBCStaticDTO.CBType,
                    CBValue = bodyBCStaticDTO.CBValue,
                };

                var result = await BarcodeService.UpdateCBStatic(username, barcodeGuid, modBarcode);

                if (!result)
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, null);
                }
                else
                {
                    apiGenericResponse.Message = "The static barcode was updated successfully.";
                    apiGenericResponse.Data = barcodeId;

                    return StatusCode(StatusCodes.Status200OK, apiGenericResponse);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, null);
        }

        #endregion **Static Barcodes**

        #region **Dynamic Barcodes**

        /// <summary>
        /// Retrieve all dynamic barcodes.
        /// </summary>
        /// <param name="pageIndex">The index of the page to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of items per page. Defaults to 10.</param>
        /// <response code="200">Succesfully. Returns an object ApiResponseDynamicBarcodes with a list of all user dynamic barcodes.</response>
        /// <response code="400">Bad request. Error in the request parameters.</response>
        /// <response code="401">Unauthorized. You are not authorized to use this method.</response>
        /// <response code="405">Method Not Allowed.</response>
        /// <response code="422">Unprocessable Entity. Unexpected API error.</response>
        /// <response code="429">Too Many Requests. The application has exceeded the API rate limit.</response>
        [SwaggerOperation(Description = "This endpoint allows clients to fetch a comprehensive list of all dynamic barcodes created by a Codis365 user. " +
          "Dynamic barcodes are barcodes that can be updated or reconfigured based on changing data or conditions. Unlike static barcodes, which remain fixed once generated, dynamic barcodes can reflect real-time information, making them useful for applications such as inventory management, ticketing systems, personalized marketing, and more. They enable better tracking, data accuracy, and flexibility by adapting to the latest information whenever scanned. This endpoint provides an efficient way to access all user dynamic barcodes information in one request. <br/><br/>" +
          "The result will be a JSON list with the available dynamic barcodes and their attributes.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = ["Codis365 Dynamic Barcodes"])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseDynamicBarcodes))]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpGet("getdynamicbarcodes")]
        [Authorize]
        public async Task<ActionResult<ApiResponseDynamicBarcodes>> GetDynamicBarcodes(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var username = SystemUtilsHelper.GetUsername();
                var barcodes = await BarcodeService.GetCBDynamicProducts(username, pageIndex, pageSize);        // Get all dynamic barcodes
                var barcodesDTO = Mapper.Map<PaginatedDynamicBarcodeList<ApiBCDynamicDTO>>(barcodes);    // Map to DTO
                var response = new ApiResponseDynamicBarcodes(true, "All user dynamic barcodes have been retrieved succesfully.", barcodesDTO);
                return Ok(response);
            }
            catch (Exception Ex)
            {
                var errorResponse = new ApiResponseDynamicBarcodes(false, Ex.Message, null!);            // Create new api response
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);                       // Return error response
            }
        }

        /// <summary>
        /// Add new dynamic barcode.
        /// </summary>
        /// <param name="bodyBCDynamicDTO">The data transfer object containing the details of the dynamic barcode to add.</param>
        /// <returns>A response indicating the result of the operation.</returns>        
        [SwaggerOperation(Description = "This endpoint allows clients to create new dynamic barcodes. Dynamic barcodes are pre-generated barcodes that do not change and can be used for various purposes such as product identification, tracking, and more. This endpoint provides an efficient way to create dynamic barcode information in one request. <br/><br/>" +
          "The response will be a JSON object containing the status of the operation and the unique ID of the generated barcode.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = ["Codis365 Dynamic Barcodes"])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseDynamicBarcodes))]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpPost("adddynamicbarcode")]
        [Authorize]
        public async Task<ActionResult<ApiGenericResponse>> AddDynamicBarcode([FromForm] BodyBCDynamicDTO bodyBCDynamicDTO)
        {
            ApiGenericResponse apiGenericResponse = new();
            try
            {
                var username = SystemUtilsHelper.GetUsername();
                var result = await BarcodeService.AddCBDynamic(username, bodyBCDynamicDTO);

                if (string.IsNullOrEmpty(result))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, null);
                }
                else
                {
                    apiGenericResponse.Message = "The new dynamic barcode was created successfully.";
                    apiGenericResponse.Data = result;

                    return StatusCode(StatusCodes.Status200OK, apiGenericResponse);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return StatusCode(StatusCodes.Status422UnprocessableEntity, apiGenericResponse);
        }


        /// <summary>
        /// Delete a dynamic barcode.
        /// </summary>
        /// <param name="barcodeId">The ID of the dynamic barcode to delete.</param>
        /// <returns>A status message indicating the result of the operation.</returns>
        [SwaggerOperation(Description = "This endpoint allows clients to delete new dynamic barcodes. Dynamic barcodes are pre-generated barcodes that do not change and can be used for various purposes such as product identification, tracking, and more. This endpoint provides an efficient way to delete dynamic barcode information in one request. <br/><br/>" +
          "The response will be a JSON object containing the status of the operation and the unique ID of the generated barcode.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = ["Codis365 Dynamic Barcodes"])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiGenericResponse))]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpDelete("deletedynamicbarcode/{barcodeId}")]
        [Authorize]
        public async Task<ActionResult<ApiGenericResponse>> DeleteDynamicBarcode(string barcodeId)
        {
            ApiGenericResponse apiGenericResponse = new();
            try
            {
                bool v = Guid.TryParse(barcodeId, out Guid barcodeGuid);
                var username = SystemUtilsHelper.GetUsername();
                var result = await BarcodeService.DeleteCBDynamicById(username, barcodeGuid);

                if (result)
                {
                    apiGenericResponse.Message = "The dynamic barcode was deleted successfully.";
                    apiGenericResponse.Data = barcodeId;

                    return StatusCode(StatusCodes.Status200OK, apiGenericResponse);
                }
                else
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, null);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, null);
        }

        /// <summary>
        /// Update dynamic barcode data.
        /// </summary>
        /// <param name="barcodeId">The ID of the dynamic barcode to be updated.</param>
        /// <param name="bodyBCDynamicDTO">The new data to be updated.</param>
        /// <returns>A status message indicating the result of the operation.</returns>
        [SwaggerOperation(Description = "This endpoint allows clients to update new dynamic barcodes. Dynamic barcodes are pre-generated barcodes that do not change and can be used for various purposes such as product identification, tracking, and more. This endpoint provides an efficient way to update dynamic barcode data information in one request. <br/><br/>" +
          "The response will be a JSON object containing the status of the operation and the unique ID of the generated barcode.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = ["Codis365 Dynamic Barcodes"])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiGenericResponse))]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpPut("updatedynamicbarcode/{barcodeId}")]
        [Authorize]
        public async Task<ActionResult<ApiGenericResponse>> UpdateDynamicBarcode(
            [FromRoute] string barcodeId,
            [FromForm] BodyBCDynamicDTO bodyBCDynamicDTO
            )
        {
            ApiGenericResponse apiGenericResponse = new();
            try
            {
                bool v = Guid.TryParse(barcodeId, out Guid barcodeGuid);
                var username = SystemUtilsHelper.GetUsername();
                var modBarcode = new BodyBCDynamicDTO
                {
                    Description = bodyBCDynamicDTO.Description,
                    CBType = bodyBCDynamicDTO.CBType,
                    CBValue = bodyBCDynamicDTO.CBValue,
                    CBShortLink = bodyBCDynamicDTO.CBShortLink,
                };

                var result = await BarcodeService.UpdateCBDynamic(username, barcodeGuid, modBarcode);

                if (!result)
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, null);
                }
                else
                {
                    apiGenericResponse.Message = "The dynamic barcode was updated successfully.";
                    apiGenericResponse.Data = barcodeId;

                    return StatusCode(StatusCodes.Status200OK, apiGenericResponse);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, null);
        }

        #endregion

    }
}
