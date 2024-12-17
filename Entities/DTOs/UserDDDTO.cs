namespace Entities.DTOs
{
    /// <summary>
    /// UserDDDTO
    /// </summary>
    public class UserDDDTO
    {
        public Guid? UserId { get; set; }
        public dynamic? DDClient { get; set; }
        public dynamic? DDModel { get; set; }
        public dynamic? DDBrand { get; set; }
        public dynamic? DDBrandName { get; set; }
        public dynamic? DDOs { get; set; }
        public dynamic? DDBrowser { get; set; }
        public dynamic? DDtype { get; set; }
        public string? IPAddress { get; set; }
        public string? DDCity { get; set; }
    }
}
