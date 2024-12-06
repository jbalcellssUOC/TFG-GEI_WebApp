using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AppChat
{
    /// <summary>
    /// Application UserId
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Row seq
    /// </summary>
    public int IdxSec { get; set; }

    /// <summary>
    /// Chat username
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Message source (0-origin, 1-destination)
    /// </summary>
    public bool Source { get; set; }

    /// <summary>
    /// Message text
    /// </summary>
    public string Message { get; set; } = null!;

    /// <summary>
    /// Message datetime
    /// </summary>
    public DateTime Datetime { get; set; }

    /// <summary>
    /// Row creation datetime
    /// </summary>
    public DateTime? IsoDateC { get; set; }

    /// <summary>
    /// Row update datetime
    /// </summary>
    public DateTime? IsoDateM { get; set; }
}
