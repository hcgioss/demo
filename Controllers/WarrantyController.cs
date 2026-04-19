using demo.Models;
using System;
using System.Linq;
using System.Web.Mvc;

public class WarrantyController : Controller
{
    AnPhatDbContext db = new AnPhatDbContext();

    public ActionResult Index()
    {
        return View(db.WarrantyTickets.ToList());
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(string serial, string issue)
    {
        var item = db.ProductItems.FirstOrDefault(x => x.serial_number == serial);

        if (item == null)
        {
            ViewBag.Error = "Không tìm thấy sản phẩm";
            return View();
        }

        var ticket = new WarrantyTicket()
        {
            warranty_code = "BH" + DateTime.Now.Ticks,
            product_item_id = item.product_item_id,
            issue_description = issue,
            status = "Pending",
            received_date = DateTime.Now,
            created_by = 1
        };

        item.status = "Warranty";

        db.WarrantyTickets.Add(ticket);
        db.SaveChanges();

        return RedirectToAction("Index");
    }

    public ActionResult UpdateStatus(int id)
    {
        var ticket = db.WarrantyTickets.Find(id);

        ticket.status = "Completed";
        ticket.completed_date = DateTime.Now;

        var item = db.ProductItems.Find(ticket.product_item_id);
        item.status = "InStock";

        db.SaveChanges();

        return RedirectToAction("Index");
    }
}