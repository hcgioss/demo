using System.Web.Mvc;
using demo.Models;
using demo.Repositories;

namespace demo.Controllers
{
    public class InventoryController : Controller
    {
        private readonly InventoryRepository _repository;

        public InventoryController()
        {
            _repository = new InventoryRepository();
        }

        // GET: Giao diện khi mới mở trang web
        [HttpGet]
        public ActionResult Index()
        {
            return View(new ProductItem());
        }

        // POST: Giao diện khi người dùng bấm nút "Thêm vào kho"
        [HttpPost]
        public ActionResult AddSerial(ProductItem model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model); // Nếu nhập thiếu, trả lại báo lỗi
            }

            if (_repository.CheckSerialExist(model.SerialNumber))
            {
                TempData["ErrorMessage"] = "Mã Serial đã tồn tại trong hệ thống!";
                return RedirectToAction("Index");
            }

            bool isSuccess = _repository.AddProductItem(model.ProductId, model.SerialNumber);

            if (isSuccess)
            {
                TempData["SuccessMessage"] = "Thêm mã Serial thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Thêm thất bại. Vui lòng kiểm tra lại hệ thống.";
            }

            return RedirectToAction("Index");
        }
    }
}