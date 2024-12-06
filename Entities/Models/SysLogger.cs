using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class SysLogger
{
    public int Id { get; set; }

    public string Project { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? Level { get; set; }

    public string? Message { get; set; }

    public string? StackTrace { get; set; }

    public string? Exception { get; set; }

    public string? Logger { get; set; }

    public string? Url { get; set; }

    /// <summary>
    /// Row creation datetime
    /// </summary>
    public DateTime? IsoDateC { get; set; }

    /// <summary>
    /// Row update datetime
    /// </summary>
    public DateTime? IsoDateM { get; set; }
}
