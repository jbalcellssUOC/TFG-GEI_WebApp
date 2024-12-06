using System;
using System.Collections.Generic;

namespace Entities.Models;

/// <summary>
/// Users Products table
/// </summary>
public partial class AppProduct
{
    /// <summary>
    /// Application UserId
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Product unique Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Product reference
    /// </summary>
    public string? Reference { get; set; }

    /// <summary>
    /// Product category
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Product description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Product price
    /// </summary>
    public decimal? Price { get; set; }

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
    /// Product ActionType
    /// </summary>
    public string? ActionId { get; set; }

    /// <summary>
    /// Row creation datetime
    /// </summary>
    public DateTime? IsoDateC { get; set; }

    /// <summary>
    /// Row update datetime
    /// </summary>
    public DateTime? IsoDateM { get; set; }

    public virtual SysActionType? Action { get; set; }

    public virtual AppUser User { get; set; } = null!;
}
