using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNC_Project.DAO;
using WebNC_Project.Models;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.IO;

namespace WebNC_Project.Areas.Server.Controllers
{
    [ServerAuthentication]
    public class RoomsController : Controller
    {
        // GET: Server/Rooms
        [ServerAuthorize("WAREHOUSE", "STAFF")]
        public async Task<ActionResult> Index(string search)
        {
            ViewBag.Search = search;
            return View(await RoomDAO.GetAll());
        }

        // GET: Server/Rooms/Details/5
        [ServerAuthorize("WAREHOUSE", "STAFF")]
        public async Task<ActionResult> Details(string id)
        {
            var result = await RoomDAO.GetByID(id);
            return View(result);
        }

        // GET: Server/Rooms/Create/5
        [ServerAuthorize]
        public async Task<ActionResult> Create()
        {
            ViewBag.ListType = await SetListType();
            return View(new Room());
        }

        [HttpPost]
        [ServerAuthorize]
        public async Task<ActionResult> Create(Room model, IEnumerable<HttpPostedFileBase> listimg)
        {
            ViewBag.ListType = await SetListType();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if(await RoomDAO.GetByID(model.ID) != null)
            {
                ModelState.AddModelError("ID", $"Room ID {model.ID} was existed");
                return View(model);
            }
            try
            {
                await RoomDAO.Create(model);
                if(listimg.ElementAtOrDefault(0) != null)
                {
                    foreach(var img in await Upload(listimg,model.ID))
                    {
                        img.RoomID = model.ID;
                        await ImageDAO.Create(img);
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Server can not create room");
                return View(model);
            }
        }

        [ServerAuthorize]
        public async Task<ActionResult> EditInfo(string id)
        {
            ViewBag.ListType = await SetListType();
            return View(await RoomDAO.GetByID(id));
        }

        [HttpPost]
        [ServerAuthorize]
        public async Task<ActionResult> EditInfo(Room model)
        {
            if (await RoomDAO.GetByID(model.ID) == null) return HttpNotFound();
            if(ModelState.IsValid)
            {
                try
                {
                    await RoomDAO.EditInfo(model);
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.ListType = await SetListType();
                    ModelState.AddModelError("", "Error when update room information");
                    return View(model);
                }
            }
            ViewBag.ListType = await SetListType();
            return View(model);
        }

        [ServerAuthorize]
        public async Task<ActionResult> EditImg(string id)
        {
            ViewBag.IDRoom = id;
            return View(await RoomDAO.GetByID(id));
        }

        [HttpPost]
        [ServerAuthorize]
        public async Task<ActionResult> EditImg(string ID, IEnumerable<HttpPostedFileBase> addnew)
        {
            var room = await RoomDAO.GetByID(ID);
            if (room == null) return HttpNotFound();
            foreach (var img in await Upload(addnew, ID))
            {
                img.RoomID = ID;
                await ImageDAO.Create(img);
            }
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public async Task<SelectList> SetListType()
        {
            return new SelectList(await RoomTypeDAO.GetAll(), "ID", "NameType");
        }

        [ChildActionOnly]
        public async Task<IEnumerable<Image>> Upload(IEnumerable<HttpPostedFileBase> listimg, string idroom)
        {
            List<Image> imgs = new List<Image>();
            foreach(var img in listimg)
            {
                FileStream stream;
                string path = Path.Combine(Server.MapPath("~/Content/img/"), img.FileName);
                img.SaveAs(path);
                stream = new FileStream(Path.Combine(path), FileMode.Open);
                imgs.Add(new Image()
                {
                    URL = await ImageDAO.Upload(stream, img.FileName, path, idroom)
                });
            }
            return imgs;
        } 

        [HttpPost]
        [ServerAuthorize]
        public async Task<ActionResult> Remove(string id)
        {
            try
            {
                var enti = await RoomDAO.GetByID(id);
                if (enti == null) return Json("Not found room type with ID: " + id, JsonRequestBehavior.AllowGet);
                await RoomDAO.Remove(id);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Server can not remove this room", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ServerAuthorize]
        public async Task<ActionResult> RemoveImg(string url)
        {
            try
            {
                var img = ImageDAO.GetByURL(url);
                if (img == null) return Json("Not found image", JsonRequestBehavior.AllowGet);
                await ImageDAO.Remove(url);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Server can not remove this image", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
