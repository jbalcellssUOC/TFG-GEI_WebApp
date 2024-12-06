using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class AppLogger
{
    /// <summary>
    /// Auto ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Application UserId
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Message of the warning or error
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Type of the message (Warning, Info, Debug, Error...)
    /// </summary>
    public string? MessageType { get; set; }

    /// <summary>
    /// Message additional info
    /// </summary>
    public string? MessageDetails { get; set; }

    /// <summary>
    /// Row creation datetime
    /// </summary>
    public DateTime? IsoDateC { get; set; }

    /// <summary>
    /// Row update datetime
    /// </summary>
    public DateTime? IsoDateM { get; set; }

    public virtual AppUser? User { get; set; }
}
