using System.ComponentModel.DataAnnotations;

namespace demo.Models
{
    public class CreateWarrantyViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã Serial")]
        [Display(Name = "Mã Serial Linh Kiện")]
        public string SerialNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập SĐT khách hàng")]
        [Display(Name = "Số Điện Thoại Khách Hàng")]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mô tả lỗi")]
        [Display(Name = "Mô tả lỗi / Triệu chứng")]
        public string IssueDescription { get; set; } = string.Empty;

        public int UserId { get; set; } = 1;   // Tạm thời hardcode, sau này lấy từ Session
    }
}