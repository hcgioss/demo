using System;
using System.ComponentModel.DataAnnotations; // Cần dòng này để bắt lỗi dữ liệu

namespace demo.Models
{
    public class ProductItem
    {
        public int ProductItemId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn linh kiện")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã Serial")]
        [StringLength(50, ErrorMessage = "Mã Serial không được vượt quá 50 ký tự")]
        public string SerialNumber { get; set; }

        public string Status { get; set; }
        public DateTime ImportDate { get; set; }
    }
}