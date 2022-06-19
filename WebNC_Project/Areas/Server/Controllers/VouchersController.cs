using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.Models;
using WebNC_Project.DAO;
using System.Threading.Tasks;

namespace WebNC_Project.Areas.Server.Controllers
{
    [ServerAuthentication]
    public class VouchersController : Controller
    {
        // GET: Server/Vouchers
        [ServerAuthorize("STAFF")]
        public async Task<ActionResult> Index(string search)
        {
            ViewBag.Search = search;
            return View(await VoucherDAO.GetAll());
        }

        // GET: Server/Vouchers/Details/5
        [ServerAuthorize("STAFF")]
        public async Task<ActionResult> Details(string id)
        {
            var result = await VoucherDAO.GetByID(id);
            return View(result);
        }

        // GET: Server/Vouchers/Create
        [ServerAuthorize]
        public ActionResult Create()
        {
            return View(new Voucher());
        }

        // POST: Server/Vouchers/Create
        [HttpPost]
        [ServerAuthorize]
        public async Task<ActionResult> Create(Voucher voucher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(CheckDate(voucher.FromDate, voucher.ToDate))
                    {
                        ModelState.AddModelError("ToDate", "The date end of voucher was not valid");
                        return View(voucher);
                    }
                    if (voucher.FromDate.Date <= DateTime.Today.Date)
                    {
                        ModelState.AddModelError("FromDate", "The date begin of voucher cannot smaller than to day");
                        return View(voucher);
                    }
                    var entity = await VoucherDAO.GetByID(voucher.Code);
                    if (entity != null)
                    {
                        ModelState.AddModelError("Code", "Code was exist, try again with new Code");
                        return View(voucher);
                    }
                    await VoucherDAO.Create(voucher);
                    return RedirectToAction("Index");
                }
                return View(voucher);
            }
            catch
            {
                ModelState.AddModelError("", "Server can not create supply");
                return View(voucher);
            }
        }

        // GET: Server/Vouchers/Edit/5
        [ServerAuthorize]
        public async Task<ActionResult> Edit(string id)
        {
            var result = await VoucherDAO.GetByID(id);
            return View(result);
        }

        // POST: Server/Vouchers/Edit/5
        [HttpPost]
        [ServerAuthorize]
        public async Task<ActionResult> Edit(Voucher voucher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (CheckDate(voucher.FromDate, voucher.ToDate))
                    {
                        ModelState.AddModelError("ToDate", "The date end of voucher was not valid");
                        return View(voucher);
                    }
                    var enti = await VoucherDAO.GetByID(voucher.Code);
                    DateTime min = DateTime.Now;
                    if (enti.FromDate.Date < DateTime.Now.Date) min = enti.FromDate.Date;
                    if(voucher.FromDate.Date < min.Date)
                    {
                        ModelState.AddModelError("FromDate", $"The date begin of voucher cannot smaller than {min.ToString("dd/MM/yyyy")}");
                        return View(voucher);
                    }
                    await VoucherDAO.Edit(voucher);
                    return RedirectToAction("Details", new { id = voucher.Code.Trim() });
                }
                return View(voucher);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(voucher);
            }
        }

        // POST: Server/Vouchers/Delete/5
        [HttpPost]
        [ServerAuthorize]
        public async Task<ActionResult> Remove(string id)
        {
            var result = VoucherDAO.GetByID(id);
            if (result == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            try
            {
                await VoucherDAO.Remove(id);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }

        private bool CheckDate(DateTime from, DateTime to)
        {
            return from.CompareTo(to) == 1 ? true : false;
        }
    }
}
