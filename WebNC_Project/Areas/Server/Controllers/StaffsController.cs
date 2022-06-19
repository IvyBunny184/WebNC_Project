using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.DAO;
using WebNC_Project.Models;

namespace WebNC_Project.Areas.Server.Controllers
{
    [ServerAuthentication, ServerAuthorize("ADMIN")]
    public class StaffsController : Controller
    {
        // GET: Server/Staffs
        public async Task<ActionResult> Index(string search)
        {
            ViewBag.Search = search;
            return View(await StaffDAO.GetAll());
        }
        private async Task<SelectList> SetListPermis()
        {
            SelectList listPer = new SelectList(await PermissionDAO.GetAll(), "ID", "Name");
            return listPer;
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.ListPer = await SetListPermis();
            return View(new Staff());
        }

        [HttpPost]
        public async Task<ActionResult> Create(Staff staff)
        {
            ViewBag.ListPer = await SetListPermis();
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await StaffDAO.GetByID(staff.ID);
                    if (entity != null) return RedirectToAction("Create");
                    await StaffDAO.Create(staff);
                    return RedirectToAction("Index");
                }
                return View(staff);
            }            
            catch
            {
                ModelState.AddModelError("", "Server can not create staff");
                return View(staff);
            }
        }

        public async Task<ActionResult> Details(string id)
        {
            var result = await StaffDAO.GetByID(id);
            return View(result);
        }

        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.ListPer = await SetListPermis();
            var result = await StaffDAO.GetByID(id);
            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Staff staff)
        {
            ViewBag.ListPer = await SetListPermis();
            try
            {
                if (ModelState.IsValid)
                {
                    await StaffDAO.Edit(staff);
                    return RedirectToAction("Details", new { id = staff.ID.Trim() });
                }
                return View(staff);
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(staff);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Remove(string id)
        {
            var result = StaffDAO.GetByID(id);
            if (result == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            try
            {
                await StaffDAO.Remove(id);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}