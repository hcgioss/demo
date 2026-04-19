using System;
using System.ComponentModel.DataAnnotations;

namespace demo.Models
{
    public class ProductItem
    {
        [Key]
        public int product_item_id { get; set; }

        public int product_id { get; set; }

        public string serial_number { get; set; }

        public string status { get; set; }

        public DateTime import_date { get; set; }
    }
}