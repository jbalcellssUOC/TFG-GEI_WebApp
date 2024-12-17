using System;
using System.Collections.Generic;

namespace Entities.Models;

/// <summary>
/// Settings and Configuration values table
/// </summary>
public partial class SysValue
{
    /// <summary>
    /// Setting Key
    /// </summary>
    public string Setting { get; set; } = null!;

    /// <summary>
    /// Seeting Value
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Key-Value description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Row creation datetime
    /// </summary>
    public DateTime? IsoDateC { get; set; }

    /// <summary>
    /// Row update datetime
    /// </summary>
    public DateTime? IsoDateM { get; set; }
}
