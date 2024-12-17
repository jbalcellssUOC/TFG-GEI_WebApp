using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AppCBDynamic
{
    /// <summary>
    /// Application UserId
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Barcode unique identification
    /// </summary>
    public Guid BarcodeId { get; set; }

    /// <summary>
    /// Barcode description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Barcode type
    /// </summary>
    public string? CBType { get; set; }

    /// <summary>
    /// Barcode value
    /// </summary>
    public string? CBValue { get; set; }

    /// <summary>
    /// Barcode shortlink
    /// </summary>
    public string? CBShortLink { get; set; }

    /// <summary>
    /// Row creation datetime
    /// </summary>
    public DateTime? IsoDateC { get; set; }

    /// <summary>
    /// Row update datetime
    /// </summary>
    public DateTime? IsoDateM { get; set; }
}
