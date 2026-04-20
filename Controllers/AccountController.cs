using System.Web.Mvc;
using demo.Models;
using demo.Repositories;

namespace demo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _repository;

        public AccountController()
        {
            _repository = new UserRepository();
        }

        // ==================== ĐĂNG NHẬP ====================
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _repository.Login(model.Username, model.Password);
                if (user != null)
                {
                    // Lưu thông tin người dùng vào Session
                    Session["UserSession"] = user;

                    // Đăng nhập thành công thì chuyển hướng về trang tra cứu bảo hành (hoặc trang chủ)
                    return RedirectToAction("Index", "Warranty");
                }
                else
                {
                    ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác!";
                }
            }
            return View(model);
        }

        // ==================== ĐĂNG KÝ ====================
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isSuccess = _repository.Register(model);
                if (isSuccess)
                {
                    TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Error = "Tên đăng nhập đã tồn tại trong hệ thống!";
                }
            }
            return View(model);
        }

        // ==================== ĐĂNG XUẤT ====================
        public ActionResult Logout()
        {
            Session.Clear(); // Xóa toàn bộ dữ liệu phiên làm việc
            return RedirectToAction("Login");
        }
    }
}