using System;
using System.Collections.Generic;

namespace Entities.Models;

/// <summary>
/// Product codes availables
/// </summary>
public partial class SysCodeType
{
    /// <summary>
    /// Code type Id
    /// </summary>
    public string CodeId { get; set; } = null!;

    /// <summary>
    /// Code description
    /// </summary>
    public string? Description { get; set; }
}
