namespace Entities.Models;

public partial class AppUsersStat
{
    /// <summary>
    /// Row auto Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Application UserId
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// SignalR connection Id
    /// </summary>
    public string? SRconnectionId { get; set; }

    /// <summary>
    /// Indicates if user is connected
    /// </summary>
    public bool? SRconnected { get; set; }

    /// <summary>
    /// Connection IPv6
    /// </summary>
    public string? IPv6 { get; set; }

    /// <summary>
    /// Connection IPv4
    /// </summary>
    public string? IPv4 { get; set; }

    /// <summary>
    /// User location
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Device Id
    /// </summary>
    public string? DevId { get; set; }

    /// <summary>
    /// Device Name
    /// </summary>
    public string? DevName { get; set; }

    /// <summary>
    /// Device OS
    /// </summary>
    public string? DevOS { get; set; }

    /// <summary>
    /// Device browser
    /// </summary>
    public string? DevBrowser { get; set; }

    /// <summary>
    /// Device brand
    /// </summary>
    public string? DevBrand { get; set; }

    /// <summary>
    /// Device brand name
    /// </summary>
    public string? DevBrandName { get; set; }

    /// <summary>
    /// Device type
    /// </summary>
    public string? DevType { get; set; }

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
