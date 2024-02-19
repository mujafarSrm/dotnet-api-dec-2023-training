
namespace DotNetAPI.Models
{
    public class Jobs
    {
        public int JobId { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? Location { get; set; }
        public decimal Salary { get; set; }
        public DateTime PostedAt { get; set; }
        public byte[]? JobImage { get; set; }
    }

}