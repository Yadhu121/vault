namespace photoCloud.Models
{
    public class profile
    {
        public int userId { get; set; }
        public string userName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfileImagePath { get; set; }
        public string? Bio { get; set; }
    }
}
