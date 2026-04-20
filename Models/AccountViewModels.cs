using System.ComponentModel.DataAnnotations;

namespace demo.Models
{
    // Model dùng cho form Đăng nhập
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
    }

    // Model dùng cho form Đăng ký
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; }

        public string Phone { get; set; }
    }

    // Model dùng để lưu Session (Phiên đăng nhập)
    public class UserSession
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
    }
}