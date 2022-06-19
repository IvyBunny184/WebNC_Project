using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.DAO;
using WebNC_Project.Models;

namespace WebNC_Project.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var rooms = await RoomDAO.GetAll();
            var vouchers = await VoucherDAO.GetAvailable();
            var model = new DemoView();
            model.Rooms = rooms.Take(4).ToList();
            model.Vouchers = vouchers.Take(4).ToList();
            return View(model);
        }

        public class DemoView
        {
            public List<Room> Rooms;
            public List<Voucher> Vouchers;
        }

        [ChildActionOnly]
        public ActionResult About()
        {
            return PartialView();
        }
    }
}

