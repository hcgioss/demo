using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using demo.Models;

namespace demo.Repositories
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // ==========================================
        // CÁC HÀM MỚI: THÊM, SỬA, XÓA (STT 1, 2, 3)
        // ==========================================

        public bool InsertProduct(Product p)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO tblProduct (product_name, price, category) VALUES (@name, @price, @cat)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", p.ProductName);
                    cmd.Parameters.AddWithValue("@price", p.Price);
                    cmd.Parameters.AddWithValue("@cat", p.Category);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateProduct(Product p)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE tblProduct SET product_name = @name, price = @price, category = @cat WHERE product_id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", p.ProductId);
                    cmd.Parameters.AddWithValue("@name", p.ProductName);
                    cmd.Parameters.AddWithValue("@price", p.Price);
                    cmd.Parameters.AddWithValue("@cat", p.Category);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM tblProduct WHERE product_id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ==========================================
        // CÁC HÀM CŨ: LẤY DANH SÁCH (CỦA BẠN GỬI)
        // ==========================================

        public List<Product> GetAllProducts()
        {
            List<Product> list = new List<Product>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT product_id, product_name, price, category FROM tblProduct";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Product
                            {
                                ProductId = Convert.ToInt32(reader["product_id"]),
                                ProductName = reader["product_name"].ToString(),
                                Price = Convert.ToDecimal(reader["price"]),
                                Category = reader["category"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public Product GetProductById(int id)
        {
            Product p = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT product_id, product_name, price, category FROM tblProduct WHERE product_id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            p = new Product
                            {
                                ProductId = Convert.ToInt32(reader["product_id"]),
                                ProductName = reader["product_name"].ToString(),
                                Price = Convert.ToDecimal(reader["price"]),
                                Category = reader["category"].ToString()
                            };
                        }
                    }
                }
            }
            return p;
        }
    }
}