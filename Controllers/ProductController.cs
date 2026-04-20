using System.Web.Mvc;
using demo.Models;
using demo.Repositories;

namespace demo.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _repo = new ProductRepository();

        // Hiển thị danh sách linh kiện hiện có
        public ActionResult Index()
        {
            var list = _repo.GetAllProducts();
            return View(list);
        }

        // Giao diện Thêm mới (STT 1)
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product p)
        {
            if (ModelState.IsValid)
            {
                if (_repo.InsertProduct(p)) return RedirectToAction("Index");
            }
            return View(p);
        }

        // Giao diện Cập nhật (STT 2)
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var p = _repo.GetProductById(id);
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                if (_repo.UpdateProduct(p)) return RedirectToAction("Index");
            }
            return View(p);
        }

        // Xử lý Xóa (STT 3)
        public ActionResult Delete(int id)
        {
            _repo.DeleteProduct(id);
            return RedirectToAction("Index");
        }
    }
}