using System;
using System.Web.Mvc;
using demo.Models;
using demo.Repositories;

namespace demo.Controllers
{
    public class Warranty2Controller : Controller
    {
        private readonly WarrantyRepository2 _repository;

        public Warranty2Controller()
        {
            _repository = new WarrantyRepository2();
        }

        // ==================== CHỨC NĂNG 13: LẬP PHIẾU BẢO HÀNH ====================
        [HttpGet]
        public ActionResult Create(string serialNumber = "")
        {
            var model = new CreateWarrantyViewModel
            {
                SerialNumber = serialNumber
            };
            return View("~/Views/Warranty/Create.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateWarrantyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Warranty/Create.cshtml", model);
            }

            bool success = _repository.CreateWarrantyTicket(
                model.SerialNumber,
                model.CustomerPhone,
                model.IssueDescription,
                model.UserId);

            if (success)
            {
                TempData["Success"] = $"✓ Lập phiếu bảo hành thành công cho Serial: {model.SerialNumber}";
                return RedirectToAction("Index", "Warranty");
            }

            TempData["Error"] = "✗ Lập phiếu thất bại. Vui lòng kiểm tra lại thông tin!";
            return View("~/Views/Warranty/Create.cshtml", model);
        }

        // ==================== CHỨC NĂNG 14: DANH SÁCH & CẬP NHẬT TRẠNG THÁI ====================
        [HttpGet]
        public ActionResult List()
        {
            var tickets = _repository.GetAllWarrantyTickets();
            return View("~/Views/Warranty/List.cshtml", tickets);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int ticketId, string newStatus)
        {
            bool success = _repository.UpdateWarrantyStatus(ticketId, newStatus);

            if (success)
                TempData["Success"] = "✓ Cập nhật trạng thái bảo hành thành công!";
            else
                TempData["Error"] = "✗ Cập nhật thất bại hoặc không tìm thấy phiếu!";

            return RedirectToAction("List");
        }
    }
}