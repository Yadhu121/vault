namespace photoCloud.Models
{
    public class User
    {
        public int userId { get; set; }
        public string userName { get; set; } = null!;
        public string passwordHash { get; set; } = null!;
        public string Email { get; set; } = null!;

    }
}
