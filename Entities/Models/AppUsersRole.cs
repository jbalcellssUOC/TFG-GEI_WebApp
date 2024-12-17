using System;
using System.Collections.Generic;

namespace Entities.Models;

/// <summary>
/// Applicaction Roles table
/// </summary>
public partial class AppUsersRole
{
    /// <summary>
    /// Role auto Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Role UserId
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Role code
    /// </summary>
    public string Role { get; set; } = null!;

    public virtual SysRole RoleNavigation { get; set; } = null!;

    public virtual AppUser User { get; set; } = null!;
}
