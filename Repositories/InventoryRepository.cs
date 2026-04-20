using System;
using System.Data.SqlClient;
using System.Configuration;
using demo.Models;

namespace demo.Repositories
{
    public class InventoryRepository
    {
        private readonly string _connectionString;

        public InventoryRepository()
        {
            // Lấy chuỗi kết nối từ file Web.config
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public bool CheckSerialExist(string serial)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM tblProductItem WHERE serial_number = @serial";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@serial", serial);
                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public bool AddProductItem(int productId, string serial)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO tblProductItem (product_id, serial_number, status, import_date) VALUES (@productId, @serial, 'InStock', @importDate)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.Parameters.AddWithValue("@serial", serial);
                    cmd.Parameters.AddWithValue("@importDate", DateTime.Now);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
        // Hàm Xuất kho (STT 5)
        public string ExportProductItem(string serial)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // 1. Kiểm tra xem mã Serial có tồn tại không và trạng thái hiện tại là gì
                string checkQuery = "SELECT status FROM tblProductItem WHERE serial_number = @serial";
                string currentStatus = null;

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@serial", serial);
                    conn.Open();
                    var result = checkCmd.ExecuteScalar();

                    if (result == null) return "NOT_FOUND"; // Không có trong CSDL
                    currentStatus = result.ToString();
                }

                // 2. Kiểm tra trạng thái: Nếu không phải InStock thì không cho xuất
                if (currentStatus != "InStock") return "ALREADY_EXPORTED";

                // 3. Tiến hành cập nhật trạng thái thành Đã xuất kho (Exported)
                string updateQuery = "UPDATE tblProductItem SET status = 'Exported' WHERE serial_number = @serial";
                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@serial", serial);
                    int rows = updateCmd.ExecuteNonQuery();
                    return rows > 0 ? "SUCCESS" : "ERROR";
                }
            }
        }
    }
}