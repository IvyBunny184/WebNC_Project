using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebNC_Project.DAO;
using WebNC_Project.ViewModel;

namespace WebNC_Project.Areas.Server.Controllers
{
    public class AccountController : Controller
    {
        // GET: Server/Account
        public ActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await StaffDAO.GetByID(model.Username);
                if(user == null)
                {
                    ModelState.AddModelError("Username", "User does not exist");
                    return View(model);
                }
                if (user.Password != model.Password)
                {
                    ModelState.AddModelError("Password", "Password incorrect");
                    return View(model);
                }
                Session["UID"] = user.ID;
                Session["Role"] = user.PermissionID;
                Session["Name"] = user.Name;
                Session["RoleName"] = user.Permission.Name;
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public ActionResult SignOut()
        {
            Session["UID"] = string.Empty;
            return RedirectToAction("Index");
        }

        public PartialViewResult UserInfo()
        {
            return PartialView();
        }
    }
}