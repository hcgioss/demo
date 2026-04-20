using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using demo.Models;

namespace demo.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // Hàm mã hóa mật khẩu sang MD5 (Đáp ứng NFR04)
        public string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        // Hàm Đăng ký
        public bool Register(RegisterViewModel model)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Kiểm tra xem username đã tồn tại chưa
                string checkQuery = "SELECT COUNT(*) FROM tblUser WHERE username = @username";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@username", model.Username);
                    conn.Open();
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0) return false; // Trùng tài khoản
                }

                // Mã hóa mật khẩu trước khi Insert
                string hashedPassword = GetMD5Hash(model.Password);

                string insertQuery = "INSERT INTO tblUser (username, password, full_name, phone, role_id) VALUES (@username, @password, @fullname, @phone, 2)"; // Mặc định role_id = 2 (Nhân viên)

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@username", model.Username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@fullname", model.FullName);
                    cmd.Parameters.AddWithValue("@phone", model.Phone ?? (object)DBNull.Value);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        // Hàm Đăng nhập
        public UserSession Login(string username, string password)
        {
            string hashedPassword = GetMD5Hash(password); // Phải mã hóa password nhập vào để so sánh với DB
            UserSession user = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT user_id, username, full_name, role_id FROM tblUser WHERE username = @username AND password = @password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserSession
                            {
                                UserId = Convert.ToInt32(reader["user_id"]),
                                Username = reader["username"].ToString(),
                                FullName = reader["full_name"].ToString(),
                                RoleId = Convert.ToInt32(reader["role_id"])
                            };
                        }
                    }
                }
            }
            return user;
        }
    }
}