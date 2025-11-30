namespace photoCloud.Models
{
    public class fileModel
    {
        public int mediaId { get; set; }
        public int userId { get; set; }
        public string filePath { get; set; } = null!;
        public string contentType { get; set; } = null!;
        public long size { get; set; }
        public DateTime uploadedAt { get; set; }
    }
}
