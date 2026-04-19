using System;

namespace demo.Models
{
    public class WarrantyViewModel
    {
        public string SerialNumber { get; set; }
        public bool IsFound { get; set; } // Dùng để check xem có tìm thấy mã không
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime ImportDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Status { get; set; } // "Còn bảo hành" hoặc "Hết hạn"
    }
}