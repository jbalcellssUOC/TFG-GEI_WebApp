using Entities.DTOs;
using Entities.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the API response DTO for dynamic barcodes.
/// </summary>
[SwaggerSchema("API response for dynamic barcodes")]
public class ApiResponseDynamicBarcodes
{
    /// <summary>
    /// ApiResponseDynamicBarcodes constructor 
    /// </summary>
    /// <param name="success"></param>
    /// <param name="message"></param>
    /// <param name="data"></param>
    public ApiResponseDynamicBarcodes(bool success, string message, PaginatedDynamicBarcodeList<ApiBCDynamicDTO> data)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the request was successful.
    /// </summary>
    [Required]
    [Display(Name = "Success", Description = "Indicates whether the request was successful.")]
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the message about the API response.
    /// </summary>
    [Required]
    [Display(Name = "Message", Description = "Provides a message about the API response.")]
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the paginated list of dynamic barcodes.
    /// </summary>
    [Required]
    [Display(Name = "Data", Description = "Contains the paginated list of dynamic barcodes.")]
    public PaginatedDynamicBarcodeList<ApiBCDynamicDTO> Data { get; set; }
}