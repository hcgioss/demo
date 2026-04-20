using System.Web.Mvc;

namespace demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // BẢO MẬT: Kiểm tra xem người dùng đã đăng nhập chưa (Biến Session có tồn tại không)
            if (Session["UserSession"] == null)
            {
                // Nếu chưa đăng nhập, tự động đá về trang Login
                return RedirectToAction("Login", "Account");
            }

            // Nếu đã đăng nhập rồi thì cho phép xem trang chủ
            return View();
        }
    }
}