using System;
using System.Collections.Generic;

namespace Entities.Models;

/// <summary>
/// Product actions types availables
/// </summary>
public partial class SysActionType
{
    /// <summary>
    /// Action Id
    /// </summary>
    public string ActionId { get; set; } = null!;

    /// <summary>
    /// Action description
    /// </summary>
    public string? Description { get; set; }

    public virtual ICollection<AppProduct> AppProducts { get; set; } = new List<AppProduct>();
}
