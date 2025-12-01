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

        public profile GetProfile(int userId)
        {
            profile p = null;
            SqlCommand cmd = new SqlCommand("profilePro", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userId", userId);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                p = new profile
                {
                    userId = Convert.ToInt32(dr["userId"]),
                    userName = dr["userName"].ToString(),
                    Email = dr["Email"].ToString(),
                    ProfileImagePath = dr["ProfileImagePath"] == DBNull.Value ? null : dr["ProfileImagePath"].ToString(),
                    Bio = dr["Bio"] == DBNull.Value ? null : dr["Bio"].ToString()
                };
            }
            con.Close();
            return p;
        }

        public void UpdateProfile(int userId, string imagePath, string bio)
        {
            SqlCommand cmd = new SqlCommand("profilePro_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userId", userId);

            if (imagePath != null)
                cmd.Parameters.AddWithValue("@imagePath", imagePath);
            else
                cmd.Parameters.AddWithValue("@imagePath", DBNull.Value);

            if (!string.IsNullOrWhiteSpace(bio))
                cmd.Parameters.AddWithValue("@Bio", bio);
            else
                cmd.Parameters.AddWithValue("@Bio", DBNull.Value);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}