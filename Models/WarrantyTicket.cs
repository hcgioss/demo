using System;
using System.ComponentModel.DataAnnotations;

namespace demo.Models
{
    public class WarrantyTicket
    {
        [Key]
        public int TicketId { get; set; }

        public string SerialNumber { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string IssueDescription { get; set; } = string.Empty;
        public string Status { get; set; } = "Đang xử lý";
        public DateTime ReceiveDate { get; set; } = DateTime.Now;
        public DateTime? ReturnDate { get; set; }
    }
}