using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using demo.Models;

namespace demo.Repositories
{
    public class ReportRepository
    {
        private string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<RevenueViewModel> GetRevenue()
        {
            var list = new List<RevenueViewModel>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT 
                        CAST(created_at AS DATE) AS Ngay,
                        SUM(total_amount - discount_amount) AS DoanhThu
                    FROM tblInvoice
                    GROUP BY CAST(created_at AS DATE)
                    ORDER BY Ngay
                ";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new RevenueViewModel
                    {
                        Date = Convert.ToDateTime(reader["Ngay"]),
                        Total = Convert.ToDecimal(reader["DoanhThu"])
                    });
                }
            }

            return list;
        }
    }
}