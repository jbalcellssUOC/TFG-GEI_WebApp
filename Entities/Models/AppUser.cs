using System;
using System.Collections.Generic;

namespace Entities.Models;

/// <summary>
/// Application User table
/// </summary>
public partial class AppUser
{
    /// <summary>
    /// User unique Id (GUID)
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// User login email
    /// </summary>
    public string Login { get; set; } = null!;

    /// <summary>
    /// User password
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// User name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// User surname
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// User phone
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// User address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Internal user comments
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Indicates if user is Admin
    /// </summary>
    public bool? IsAdmin { get; set; }

    /// <summary>
    /// Indicates if user has 2FA login activated
    /// </summary>
    public bool? Is2FAEnabled { get; set; }

    /// <summary>
    /// Indicates if user is blocked
    /// </summary>
    public bool? IsBlocked { get; set; }

    /// <summary>
    /// JWT Login token
    /// </summary>
    public string? TokenID { get; set; }

    /// <summary>
    /// Token issued datetime (UTC)
    /// </summary>
    public DateTime? TokenIssuedUTC { get; set; }

    /// <summary>
    /// Token expire datetime (UTC)
    /// </summary>
    public DateTime? TokenExpiresUTC { get; set; }

    /// <summary>
    /// Indicates if token is valid
    /// </summary>
    public bool? TokenIsValid { get; set; }

    /// <summary>
    /// User login retries
    /// </summary>
    public int? Retries { get; set; }

    /// <summary>
    /// JWT API Token (Secret)
    /// </summary>
    public string? APIToken { get; set; }

    /// <summary>
    /// Row creation datetime
    /// </summary>
    public DateTime? IsoDateC { get; set; }

    /// <summary>
    /// Row update datetime
    /// </summary>
    public DateTime? IsoDateM { get; set; }

    public virtual ICollection<AppLogger> AppLoggers { get; set; } = new List<AppLogger>();

    public virtual ICollection<AppProduct> AppProducts { get; set; } = new List<AppProduct>();

    public virtual ICollection<AppUsersRole> AppUsersRoles { get; set; } = new List<AppUsersRole>();

    public virtual ICollection<AppUsersStat> AppUsersStats { get; set; } = new List<AppUsersStat>();
}
