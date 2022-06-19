using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.DAO;
using WebNC_Project.Models;
using PagedList;
using PagedList.Mvc;

namespace WebNC_Project.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Service
        public async Task<ActionResult> Index(int? page, string search)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            IEnumerable<Service> result;
            if(search == null)
            {
                result = await ServiceDAO.GetAll();
            }
            else
            {
                result = await ServiceDAO.Search(search);
            }
            return View(result.ToPagedList(pageNum,pageSize));
        }

        public async Task<ActionResult> Details(string id)
        {
            var result = await ServiceDAO.GetByID(id);
            return View(result);
        }
    }
}