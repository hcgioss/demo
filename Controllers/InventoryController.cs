using System.Web.Mvc;
using demo.Models;
using demo.Repositories;

namespace demo.Controllers
{
    public class InventoryController : Controller
    {
        private readonly InventoryRepository _inventoryRepo;
        private readonly ProductRepository _productRepo; // Thêm biến kết nối bảng Sản phẩm

        public InventoryController()
        {
            _inventoryRepo = new InventoryRepository();
            _productRepo = new ProductRepository(); // Khởi tạo
        }

        // GET: Giao diện khi mới mở trang web
        [HttpGet]
        public ActionResult Index()
        {
            // Lấy toàn bộ danh sách linh kiện từ DB
            var productList = _productRepo.GetAllProducts();

            // Đóng gói thành SelectList và ném vào ViewBag để View sử dụng
            ViewBag.ProductList = new SelectList(productList, "ProductId", "ProductName");

            return View(new ProductItem());
        }

        // POST: Khi người dùng bấm xác nhận nhập kho
        [HttpPost]
        public ActionResult AddSerial(ProductItem model)
        {
            if (!ModelState.IsValid)
            {
                // Nếu bị lỗi (như để trống), phải load lại danh sách thả xuống
                var productList = _productRepo.GetAllProducts();
                ViewBag.ProductList = new SelectList(productList, "ProductId", "ProductName");
                return View("Index", model);
            }

            if (_inventoryRepo.CheckSerialExist(model.SerialNumber))
            {
                TempData["ErrorMessage"] = "Mã Serial đã tồn tại trong hệ thống!";
                return RedirectToAction("Index");
            }

            bool isSuccess = _inventoryRepo.AddProductItem(model.ProductId, model.SerialNumber);

            if (isSuccess)
            {
                TempData["SuccessMessage"] = "Thêm mã Serial thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Thêm thất bại. Vui lòng kiểm tra lại.";
            }

            return RedirectToAction("Index");
        }
        // GET: Giao diện Quét mã Xuất kho
        [HttpGet]
        public ActionResult Export()
        {
            return View();
        }

        // POST: Xử lý khi nhân viên bấm "Xác nhận xuất kho"
        [HttpPost]
        public ActionResult Export(string serialNumber)
        {
            if (string.IsNullOrEmpty(serialNumber))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập hoặc quét mã Serial!";
                return RedirectToAction("Export");
            }

            // Gọi hàm xử lý dưới DB
            string result = _inventoryRepo.ExportProductItem(serialNumber);

            if (result == "NOT_FOUND")
            {
                TempData["ErrorMessage"] = "Mã Serial không tồn tại trong hệ thống!";
            }
            else if (result == "ALREADY_EXPORTED")
            {
                TempData["ErrorMessage"] = "Linh kiện này đã được xuất khỏi kho từ trước!";
            }
            else if (result == "SUCCESS")
            {
                TempData["SuccessMessage"] = "Xuất kho THÀNH CÔNG mã Serial: " + serialNumber;
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi hệ thống, vui lòng thử lại!";
            }

            return RedirectToAction("Export");
        }
    }
}