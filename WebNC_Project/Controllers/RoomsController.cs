using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.Models;
using WebNC_Project.DAO;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;

namespace WebNC_Project.Controllers
{
    public class RoomsController : Controller
    {
        // GET: Rooms
        [Obsolete]
        public async Task<ActionResult> Index(int? page, string search, DateTime? datein, DateTime? dateout)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            IEnumerable<Room> result;
            if(search != null || datein.HasValue && dateout.HasValue)
            {
                if(search != null)
                {
                    result = await RoomDAO.Search(search);
                }
                else
                {
                    result = await RoomDAO.GetRoomAvailable(datein.Value, dateout.Value);
                }
            }
            else
            {
                result = await RoomDAO.GetAll();
            }
            return View(result.ToPagedList(pageNum,pageSize));
        }

        public async Task<ActionResult> Details(string id)
        {
            Room room = await RoomDAO.GetByID(id);
            return View(room);
        }
    }
}