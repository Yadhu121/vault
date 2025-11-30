using System.Data;
using System.Data.SqlClient;

namespace photoCloud.Models
{
    public class DBConnect
    {
        SqlConnection con = new SqlConnection(@"server=DESKTOP-B435JNF\SQLEXPRESS;database=photoCloud;integrated security = true");

        public int insertUser(User user)
        {
            SqlCommand cmd = new SqlCommand("insertIntoUsers", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userName", user.userName);
            cmd.Parameters.AddWithValue("@passwordHash", user.passwordHash);
            cmd.Parameters.AddWithValue("@Email", user.Email);

            con.Open();
            int Status = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            return Status;
        }

        public int loginUser(string logininput, string pass)
        {
            SqlCommand cmd = new SqlCommand("login", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@loginInput", logininput);
            cmd.Parameters.AddWithValue("@passwordHash", pass);

            con.Open();
            int Status = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            return Status;
        }

        public int insertMedia(int userId, string filepath, string contentType, long size)
        {
            SqlCommand cmd = new SqlCommand("insertMedia", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userid", userId);
            cmd.Parameters.AddWithValue("@filePath", filepath);
            cmd.Parameters.AddWithValue("@contentType", contentType);
            cmd.Parameters.AddWithValue("@size", size);

            con.Open();
            int Status = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            return Status;
        }

        public List<fileModel> getFilesByUser(int userId)
        {
            List<fileModel> files = new List<fileModel>();

            SqlCommand cmd = new SqlCommand(
                "select mediaId, userId, filePath, contentType, size, uploadedAt from files where userId = @userId order by uploadedAt desc",
                con
            );
            cmd.Parameters.AddWithValue("@userId", userId);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                fileModel f = new fileModel
                {
                    mediaId = Convert.ToInt32(reader["mediaId"]),
                    userId = Convert.ToInt32(reader["userId"]),
                    filePath = reader["filePath"].ToString()!,
                    contentType = reader["contentType"].ToString()!,
                    size = Convert.ToInt64(reader["size"]),
                    uploadedAt = Convert.ToDateTime(reader["uploadedAt"])
                };
                files.Add(f);
            }
            reader.Close();
            con.Close();

            return files;

        }
    }
}