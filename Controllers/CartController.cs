using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using demo.Models;
using demo.Repositories;

namespace demo.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductRepository _repo = new ProductRepository();

        // Hiển thị màn hình bán hàng
        public ActionResult Index()
        {
            POSViewModel model = new POSViewModel();
            model.AvailableProducts = _repo.GetAllProducts();

            // Lấy giỏ hàng từ Session
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<CartItem>();
            }
            model.CartItems = (List<CartItem>)Session["Cart"];

            return View(model);
        }

        // Thêm linh kiện vào giỏ
        public ActionResult AddToCart(int productId)
        {
            List<CartItem> cart = (List<CartItem>)Session["Cart"] ?? new List<CartItem>();

            // Kiểm tra xem linh kiện đã có trong giỏ chưa
            CartItem existingItem = cart.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                Product p = _repo.GetProductById(productId);
                if (p != null)
                {
                    cart.Add(new CartItem
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        UnitPrice = p.Price,
                        Quantity = 1,
                        Category = p.Category
                    });
                }
            }

            Session["Cart"] = cart;
            return RedirectToAction("Index");
        }

        // Xóa giỏ hàng
        public ActionResult ClearCart()
        {
            Session["Cart"] = new List<CartItem>();
            return RedirectToAction("Index");
        }

        public ActionResult PrintInvoice()
        {
            var cartItems = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

            var model = new POSViewModel
            {
                CartItems = cartItems
            };

            return View("InvoicePrint", model);
        }
    }
}