using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.DAO;
using WebNC_Project.Models;
using System.Threading.Tasks;

namespace WebNC_Project.Areas.Server.Controllers
{
    [ServerAuthentication]
    public class ServicesController : Controller
    {
        // GET: Server/Services
        [ServerAuthorize("STAFF")]
        public async Task<ActionResult> Index(string search)
        {
            ViewBag.Search = search;
            return View(await ServiceDAO.GetAll());
        }

        // GET: Server/Services/Details/5
        [ServerAuthorize("STAFF")]
        public async Task<ActionResult> Details(string id)
        {
            Service result = await ServiceDAO.GetByID(id);
            return View(result);
        }

        // GET: Server/Services/Create
        [ServerAuthorize]
        public ActionResult Create()
        {
            return View(new Service());
        }

        // POST: Server/Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [ServerAuthorize]
        public async Task<ActionResult> Create(Service service)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await ServiceDAO.GetByID(service.ID);
                    if (entity != null)
                    {
                        ModelState.AddModelError(service.ID, $"The services {service.ID} existed");
                        return View(service);
                    }
                    await ServiceDAO.Create(service);
                    return RedirectToAction("Index");
                }
                return View(service);
            }
            catch
            {
                ModelState.AddModelError("", "Server can not create services");
                return View(service);
            }
        }

        // GET: Server/Services/Edit/5
        [ServerAuthorize]
        public async Task<ActionResult> Edit(string id)
        {
            Service service = await ServiceDAO.GetByID(id);
            return View(service);
        }

        // POST: Server/Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [ServerAuthorize]
        public async Task<ActionResult> Edit(Service service)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await ServiceDAO.Edit(service);
                    return RedirectToAction("Details", new { id = service.ID.Trim() });
                }
                return View(service);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(service);
            }
        }

        // POST: Server/Services/Delete/5
        [HttpPost]
        [ServerAuthorize]
        public async Task<ActionResult> Remove(string id)
        {
            var result = await ServiceDAO.GetByID(id);
            if (result == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            try
            {
                await ServiceDAO.Remove(id);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
