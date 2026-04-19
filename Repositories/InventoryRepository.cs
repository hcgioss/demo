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
    }
}