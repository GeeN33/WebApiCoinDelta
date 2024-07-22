namespace WebApiCoinDelta.Models
{
    public class Logger
    {
        public int Id { get; set; }
        public string Type { get; set; } = "unknown";
        public string Description { get; set; } = "unknown";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
