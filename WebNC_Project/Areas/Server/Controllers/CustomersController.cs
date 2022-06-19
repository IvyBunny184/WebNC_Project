using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using WebNC_Project.Models;
using WebNC_Project.DAO;
using System.Threading.Tasks;

namespace WebNC_Project.Areas.Server.Controllers
{
    [ServerAuthentication]
    public class CustomersController : Controller
    {
        // GET: Server/Customers
        [ServerAuthorize("ADMIN", "STAFF")]
        public async Task<ActionResult> Index(string search)
        {
            ViewBag.Search = search;
            return View(await CustomerDAO.GetAll());
        }

        // GET: Server/Customers/Details/5
        [ServerAuthorize("ADMIN", "STAFF")]
        public async Task<ActionResult> Details(string id)
        {
            Customer cus = await CustomerDAO.GetByID(id);
            return View(cus);
        }

        // GET: Server/Customers/Create
        [ServerAuthorize("ADMIN")]
        public ActionResult Create()
        {
            return View(new Customer());
        }

        // POST: Server/Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServerAuthorize("ADMIN")]
        public async Task<ActionResult> Create(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await CustomerDAO.GetByID(customer.ID);
                    if (entity != null)
                    {
                        ModelState.AddModelError("", $"The username {customer.ID} existed");
                        return View(customer);
                    }
                    await CustomerDAO.Create(customer);
                    return RedirectToAction("Index");
                }
                return View(customer);
            }
            catch
            {
                ModelState.AddModelError("", "Server can not create customer");
                return View(customer);
            }
        }

        // GET: Server/Customers/Edit/5
        [ServerAuthorize("ADMIN")]
        public async Task<ActionResult> Edit(string id)
        {
            Customer cus = await CustomerDAO.GetByID(id);
            return View(cus);
        }

        // POST: Server/Customers/Edit/5
        [HttpPost]
        [ServerAuthorize("ADMIN")]
        public async Task<ActionResult> Edit(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await CustomerDAO.Edit(customer);
                    return RedirectToAction("Details", new { id = customer.ID.Trim() });
                }
                return View(customer);
            }
            catch
            {
                ModelState.AddModelError("", "Server can not update customer");
                return View();
            }
        }


        // POST: Server/Customers/Delete/5
        [HttpPost]
        [ServerAuthorize("ADMIN")]
        public async Task<ActionResult> Remove(string id)
        {
            var result = await CustomerDAO.GetByID(id);
            if (result == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            try
            {
                await CustomerDAO.Remove(id);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
