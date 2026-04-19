using System;
using System.Data.SqlClient;
using System.Configuration;
using demo.Models;

namespace demo.Repositories
{
    public class WarrantyRepository
    {
        private readonly string _connectionString;

        public WarrantyRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public WarrantyViewModel CheckWarranty(string serial)
        {
            WarrantyViewModel result = new WarrantyViewModel { SerialNumber = serial, IsFound = false };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Truy vấn tìm ngày nhập kho của Serial này
                string query = "SELECT product_id, import_date FROM tblProductItem WHERE serial_number = @serial";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@serial", serial);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.IsFound = true;
                            result.ProductId = Convert.ToInt32(reader["product_id"]);
                            result.ImportDate = Convert.ToDateTime(reader["import_date"]);

                            // Giả sử linh kiện mặc định bảo hành 24 tháng (2 năm)
                            result.ExpirationDate = result.ImportDate.AddMonths(24);

                            // So sánh với ngày hiện tại để ra trạng thái
                            if (result.ExpirationDate >= DateTime.Now)
                                result.Status = "Còn bảo hành";
                            else
                                result.Status = "Đã hết hạn";

                            // Gắn tạm tên sản phẩm dựa vào ID để hiển thị cho đẹp
                            if (result.ProductId == 1) result.ProductName = "CPU Intel Core i5 13400F";
                            else if (result.ProductId == 2) result.ProductName = "Mainboard ASUS B760M";
                            else result.ProductName = "Linh kiện khác";
                        }
                    }
                }
            }
            return result;
        }
    }
}