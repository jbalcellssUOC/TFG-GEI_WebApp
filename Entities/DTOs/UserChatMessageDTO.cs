namespace Entities.DTOs
{
    /// <summary>
    /// UserChatMessageDTO
    /// </summary>
    public class UserChatMessageDTO
    {
        public int IdxSec { get; set; }
        public string? UserName { get; set; }
        public bool Source { get; set; }
        public string? Message { get; set; }
        public DateTime Datetime { get; set; }
    }
}
