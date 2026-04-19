using System.Linq;
using System.Web.Mvc;

public class ReportController : Controller
{
    AnPhatDbContext db = new AnPhatDbContext();

    public ActionResult Dashboard()
    {
        ViewBag.TotalWarranty = db.WarrantyTickets.Count();
        ViewBag.InStock = db.ProductItems.Count(x => x.status == "InStock");
        ViewBag.InWarranty = db.ProductItems.Count(x => x.status == "Warranty");

        return View();
    }
}