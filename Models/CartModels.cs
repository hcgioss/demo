using System.Collections.Generic;
using System.Linq;

namespace demo.Models
{
    // Đại diện cho 1 linh kiện trong giỏ hàng
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }

        public decimal TotalPrice { get { return UnitPrice * Quantity; } }
    }

    // Đại diện cho toàn bộ màn hình Bán hàng
    public class POSViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public List<Product> AvailableProducts { get; set; }

        public decimal SubTotal { get { return CartItems != null ? CartItems.Sum(x => x.TotalPrice) : 0; } }

        // Logic tính Combo Build PC: Nếu mua từ 3 món trở lên, giảm 5%
        public decimal Discount
        {
            get
            {
                if (CartItems != null && CartItems.Count >= 3)
                {
                    return SubTotal * 0.05m; // Giảm 5%
                }
                return 0;
            }
        }

        public decimal FinalTotal { get { return SubTotal - Discount; } }
    }

    // Model dùng để map với bảng tblProduct trong DB
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}