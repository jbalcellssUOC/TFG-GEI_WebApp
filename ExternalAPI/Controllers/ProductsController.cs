using Asp.Versioning;
using AutoMapper;
using BusinessLogicLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using Entities.POCO;
using ExternalAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Swashbuckle.AspNetCore.Annotations;

namespace ExternalAPI.Controllers
{
    /// <summary>
    /// ProductsController class
    /// </summary>
    /// <param name="ProductService"></param>
    /// <param name="Mapper"></param>
    /// <param name="SystemUtilsHelper"></param>
    [ApiVersion(1.0)]
    [ApiController]
    [Route("v1/[controller]")]
    public class ProductsController(
        IProductService ProductService,
        IMapper Mapper,
        ISystemUtilsHelper SystemUtilsHelper
        ) : ControllerBase
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /******************************************************************************/
        /**************** PRIVATE METHODS (FOR AUTHENTICATED USERS) *******************/
        /******************************************************************************/

        #region **Products**

        /// <summary>
        /// Retrieve all products.
        /// </summary>
        /// <param name="pageIndex">The index of the page to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of items per page. Defaults to 10.</param>
        /// <response code="200">Succesfully. Returns an ApiResponseProducts object with an array of all user products.</response>
        /// <response code="400">Bad request. Error in the request parameters.</response>
        /// <response code="401">Unauthorized. You are not authorized to use this method.</response>
        /// <response code="405">Method Not Allowed.</response>
        /// <response code="422">Unprocessable Entity. Unexpected API error.</response>
        /// <response code="429">Too Many Requests. The application has exceeded the API rate limit.</response>
        [SwaggerOperation(Description = "Retrieve all products associated with Codis365 user. The result will be a JSON list with the available products and their attributes.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = ["Codis365 Products"])]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponseProducts))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpGet("getproducts")]
        [Produces("application/json")]
        [Authorize]
        public async Task<ActionResult<ApiResponseProducts>> GetProducts(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var username = SystemUtilsHelper.GetUsername();
                var products = await ProductService.GetProducts(username, pageIndex, pageSize);                                     // Get all static barcodes
                var productsDTO = Mapper.Map<PaginatedProductList<ApiProductDTO>>(products);                                        // Map to DTO
                var response = new ApiResponseProducts(true, "All user products have been retrieved succesfully.", productsDTO);
                return Ok(response);
            }
            catch (Exception Ex)
            {
                var errorResponse = new ApiResponseProducts(false, Ex.Message, null!);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
        }

        /// <summary>
        /// Add a new product.
        /// </summary>
        /// <param name="BodyProductDTO">The data transfer object containing the details of the product to add.</param>
        /// <returns>A response indicating the result of the operation.</returns>        
        [SwaggerOperation(Description = "This endpoint allows clients to create new product.<br/><br/>" +
          "The response will be a JSON object containing the status of the operation and the unique ID of the generated product.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = ["Codis365 Products"])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiGenericResponse))]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpPost("addproduct")]
        [Authorize]
        public async Task<ActionResult<ApiGenericResponse>> AddProduct([FromForm] BodyProductDTO BodyProductDTO)
        {
            ApiGenericResponse apiGenericResponse = new();
            try
            {
                var username = SystemUtilsHelper.GetUsername();
                var result = await ProductService.AddProduct(username, BodyProductDTO);
                if (string.IsNullOrEmpty(result))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, null);
                }
                else
                {
                    apiGenericResponse.Message = "The new product was created successfully.";
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
        /// Delete a product.
        /// </summary>
        /// <returns>A status message indicating the result of the operation.</returns>
        [SwaggerOperation(Description = "This endpoint allows clients to delete products. <br/><br/>" +
          "The response will be a JSON object containing the status of the operation and the unique ID of the deleted product.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = ["Codis365 Products"])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiGenericResponse))]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpDelete("deleteproduct/{productId}")]
        [Authorize]
        public async Task<ActionResult<ApiGenericResponse>> DeleteProduct(string productId)
        {
            ApiGenericResponse apiGenericResponse = new();
            try
            {
                Guid.TryParse(productId, out Guid productGuid);
                var username = SystemUtilsHelper.GetUsername();
                var result = await ProductService.DeleteProductById(username, productGuid);
                if (result)
                {
                    apiGenericResponse.Message = "The product was deleted successfully.";
                    apiGenericResponse.Data = productId;

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
        /// Update product data.
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(Description = "This endpoint allows clients to update product data.<br/><br/>" +
          "The response will be a JSON object containing the status of the operation and the unique ID of the product.<br /><br />" +
          "<p id=\"headerBorder\"><strong>This is a secure method so you will have to use JWT Bearer type authentication in the composition of the request headers.</strong></p>",
          Tags = ["Codis365 Products"])]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiGenericResponse))]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status405MethodNotAllowed)]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpPut("updateproduct/{productId}")]
        [Authorize]
        public async Task<ActionResult<ApiGenericResponse>> UpdateProduct(
            [FromRoute] string productId,
            [FromForm] BodyProductDTO bodyProductDTO)
        {
            ApiGenericResponse apiGenericResponse = new();
            try
            {
                Guid.TryParse(productId, out Guid productGuid);
                var username = SystemUtilsHelper.GetUsername();
                var result = await ProductService.UpdateProduct(username, productGuid, bodyProductDTO);
                if (!result)
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, null);
                }
                else
                {
                    apiGenericResponse.Message = "The product was updated successfully.";
                    apiGenericResponse.Data = productId;

                    return StatusCode(StatusCodes.Status200OK, apiGenericResponse);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"{System.Reflection.MethodBase.GetCurrentMethod()!.Name}[M]: {ex.Message ?? string.Empty}[StackT]: {ex.StackTrace ?? string.Empty}[HLink]: {ex.HelpLink ?? string.Empty}[HResult]: {ex.HResult}[Source]: {ex.Source ?? string.Empty}[Data]: {(ex.Data != null ? string.Join(", ", ex.Data.Keys.Cast<object>().Select(key => $"{key}: {ex.Data[key]}")) : string.Empty)}[InnerE]: {ex.InnerException?.Message ?? string.Empty}");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, null);
        }

        #endregion **Products**
    }
}
