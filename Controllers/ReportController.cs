using System.Linq;
using System.Web.Mvc;
using demo.Repositories;
using demo.Models;

namespace demo.Controllers
{
    public class ReportController : Controller
    {
        private ReportRepository repo = new ReportRepository();

        public ActionResult Revenue()
        {
            var data = repo.GetRevenue();
            ViewBag.TotalRevenue = data.Sum(x => x.Total);
            return View(data);
        }
    }
}