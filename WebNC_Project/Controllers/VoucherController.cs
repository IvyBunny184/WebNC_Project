using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using WebNC_Project.DAO;
using WebNC_Project.Models;

namespace WebNC_Project.Controllers
{
    public class VoucherController : Controller
    {
        // GET: Voucher
        public async Task<ActionResult> Index(int? page, string search)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            IEnumerable<Voucher> result;
            if (search == null)
            {
                result = await VoucherDAO.GetAvailable();
            }
            else
            {
                result = await VoucherDAO.Search(search);
            }
            return View(result.ToPagedList(pageNum,pageSize));
        }
    }
}