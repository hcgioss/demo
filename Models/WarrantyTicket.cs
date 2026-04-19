using System;

namespace demo.Models
{
    public class WarrantyTicket
    {
        public int warranty_id { get; set; }
        public string warranty_code { get; set; }
        public int product_item_id { get; set; }
        public string issue_description { get; set; }
        public string status { get; set; }
        public DateTime received_date { get; set; }
        public DateTime? completed_date { get; set; }
        public int created_by { get; set; }
    }
}