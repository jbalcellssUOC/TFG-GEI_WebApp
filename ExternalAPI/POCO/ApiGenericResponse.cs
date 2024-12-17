using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the API generic response for several operations.
/// </summary>
[SwaggerSchema("API response for generic response")]
public class ApiGenericResponse
{
    /// <summary>Provides a message about the operation response.</summary>
    /// <example>The static barcode has been [created/updated/deleted] successfully.</example>
    [Required]
    [Display(Name = "Message", Description = "Provides a message about the operation response.")]
    public string? Message { get; set; }

    /// <summary>Contains the result data of the operation.</summary>
    /// <example>f47ac10b-58cc-4372-a567-0e02b2c3d479</example>
    [Required]
    [Display(Name = "Data", Description = "Contains the result data of the operation.")]
    public string? Data { get; set; }
}
