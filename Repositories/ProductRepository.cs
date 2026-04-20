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