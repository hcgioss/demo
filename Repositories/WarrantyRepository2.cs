using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using demo.Models;

namespace demo.Repositories
{
    public class WarrantyRepository2
    {
        private readonly string _connectionString;

        public WarrantyRepository2()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["AnPhatDB_Final"].ConnectionString;
        }

        // ==================== CHỨC NĂNG 13: LẬP PHIẾU BẢO HÀNH ====================
        public bool CreateWarrantyTicket(string serialNumber, string customerPhone, string issueDescription, int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = @"
                        INSERT INTO tblWarrantyTicket 
                            (serial_number, customer_phone, user_id, issue_description, status, receive_date)
                        VALUES 
                            (@serial, @phone, @userId, @issue, N'Đang xử lý', GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@serial", serialNumber);
                        cmd.Parameters.AddWithValue("@phone", customerPhone);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@issue", issueDescription);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi CreateWarrantyTicket: " + ex.Message);
                return false;
            }
        }

        // ==================== CHỨC NĂNG 14: LẤY DANH SÁCH PHIẾU BẢO HÀNH ====================
        public List<WarrantyTicketViewModel> GetAllWarrantyTickets()
        {
            var list = new List<WarrantyTicketViewModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        ticket_id AS TicketId,
                        serial_number AS SerialNumber,
                        customer_phone AS CustomerPhone,
                        issue_description AS IssueDescription,
                        status AS TicketStatus,
                        receive_date AS ReceiveDate,
                        return_date AS ReturnDate
                    FROM tblWarrantyTicket 
                    ORDER BY receive_date DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ticket = new WarrantyTicketViewModel
                            {
                                TicketId = Convert.ToInt32(reader["TicketId"]),
                                SerialNumber = reader["SerialNumber"].ToString(),
                                CustomerPhone = reader["CustomerPhone"].ToString(),
                                IssueDescription = reader["IssueDescription"].ToString(),
                                TicketStatus = reader["TicketStatus"].ToString(),
                                ReceiveDate = Convert.ToDateTime(reader["ReceiveDate"]),
                                ReturnDate = reader["ReturnDate"] == DBNull.Value
                                             ? (DateTime?)null
                                             : Convert.ToDateTime(reader["ReturnDate"])
                            };
                            list.Add(ticket);
                        }
                    }
                }
            }
            return list;
        }

        // ==================== CHỨC NĂNG 14: CẬP NHẬT TRẠNG THÁI BẢO HÀNH ====================
        public bool UpdateWarrantyStatus(int ticketId, string newStatus)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = @"
                        UPDATE tblWarrantyTicket 
                        SET 
                            status = @newStatus,
                            return_date = CASE 
                                            WHEN @newStatus IN (N'Hoàn thành', N'Đã trả máy', N'Trả máy') 
                                            THEN GETDATE() 
                                            ELSE return_date 
                                          END
                        WHERE ticket_id = @ticketId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@newStatus", newStatus);
                        cmd.Parameters.AddWithValue("@ticketId", ticketId);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi UpdateWarrantyStatus: " + ex.Message);
                return false;
            }
        }
    }
}