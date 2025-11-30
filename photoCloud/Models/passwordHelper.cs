using System.Security.Cryptography;
using System.Text;

namespace photoCloud.Models
{
    public class passwordHelper
    {
        public static string hashPassword(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
