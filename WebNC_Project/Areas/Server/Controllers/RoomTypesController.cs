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
    [ServerAuthentication, ServerAuthorize]
    public class RoomTypesController : Controller
    {
        // GET: Server/RoomTypes
        public ActionResult Index(string search)
        {
            ViewBag.Search = search;
            return View(new RoomType());
        }

        public async Task<JsonResult> GetList()
        {
            List<ModelJson> src = new List<ModelJson>();
            foreach(var item in await RoomTypeDAO.GetAll())
            {
                src.Add(new ModelJson() { ID = item.ID, NameType = item.NameType, Count = item.Rooms.Count });
            }
            return Json(new { data = src }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView(new RoomType());
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoomType model)
        {
            try
            {
                if (!ModelState.IsValid) return PartialView(model);
                var enti = await RoomTypeDAO.GetByID(model.ID);
                if (enti != null)
                {
                    ModelState.AddModelError("ID", "ID was existed");
                    return PartialView(model);
                }
                await RoomTypeDAO.Create(model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var enti = await RoomTypeDAO.GetByID(id);
            return PartialView(enti);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoomType model)
        {
            try
            {
                if (!ModelState.IsValid) return PartialView(model);
                var enti = await RoomTypeDAO.GetByID(model.ID);
                if(enti == null)
                {
                    ModelState.AddModelError("", "Room type does not existed");
                    return PartialView(model);
                }
                await RoomTypeDAO.Edit(model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Remove(string id)
        {
            try
            {
                var enti = await RoomTypeDAO.GetByID(id);
                if (enti == null) return Json("Not found room type with ID: " + id,JsonRequestBehavior.AllowGet);
                if (enti.Rooms.Count() > 0) return Json("Can not remove this room type cause have room in this", JsonRequestBehavior.AllowGet);
                await RoomTypeDAO.Remove(id);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Server can not remove this room type", JsonRequestBehavior.AllowGet);
            }
        }

        struct ModelJson
        {
            public string ID;
            public string NameType;
            public int Count;
        }
    }
}