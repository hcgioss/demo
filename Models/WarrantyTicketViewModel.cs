using System;
using System.Collections.Generic;

namespace demo.Models
{
    public class WarrantyTicketViewModel
    {
        public int TicketId { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string IssueDescription { get; set; } = string.Empty;
        public string TicketStatus { get; set; } = string.Empty;
        public DateTime ReceiveDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // Danh sách tất cả phiếu (dùng cho trang List)
        public List<WarrantyTicketViewModel> Tickets { get; set; } = new List<WarrantyTicketViewModel>();
    }
}