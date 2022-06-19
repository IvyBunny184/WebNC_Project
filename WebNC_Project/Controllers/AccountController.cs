using System.Threading.Tasks;
using System.Web.Mvc;
using WebNC_Project.ViewModel;
using WebNC_Project.Models;
using WebNC_Project.DAO;
using WebNC_Project.App_Start;

namespace WebNC_Project.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account

        [CustomerAuthentication]
        public ActionResult Index()
        {
            return RedirectToAction("EditInfo");
        }

        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);
            Customer user = await CustomerDAO.GetByID(model.Username);
            if (user == null)
            {
                ModelState.AddModelError("Username", "User does not exist");
                return View(model);
            }
            if (user.Password != model.Password)
            {
                ModelState.AddModelError("Password", "Password incorrect");
                return View(model);
            }
            Session["Customer"] = user.ID;
            Session["NameCus"] = user.Name;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View(new Customer());
        }

        [HttpPost]
        public async Task<ActionResult> Register(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await CustomerDAO.GetByID(customer.ID);
                    if (entity != null)
                    {
                        ModelState.AddModelError("ID", $"The username {customer.ID} existed");
                        return View(customer);
                    }
                    await CustomerDAO.Create(customer);
                    return RedirectToAction("Login");
                }
                return View(customer);
            }
            catch
            {
                ModelState.AddModelError("", "Server can not create customer");
                return View(customer);
            }
        }

        public ActionResult Logout()
        {
            Session["Customer"] = null;
            Session["NameCus"] = null;
            return RedirectToAction("Index", "Home");
        }

        [CustomerAuthentication]
        public async Task<ActionResult> EditInfo()
        {
            Customer cus = await CustomerDAO.GetByID((string)Session["Customer"]);
            if (cus == null) return RedirectToAction("Login");
            return View(cus);
        }

        [HttpPost, CustomerAuthentication]
        public async Task<ActionResult> EditInfo(Customer model) 
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                await CustomerDAO.Edit(model);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError("ID", "Server can not update info");
                return View(model);
            }
        }

        [CustomerAuthentication]
        public ActionResult EditPass()
        {
            if (Session["Customer"] == null) return RedirectToAction("Login");
            return View(new ResetPassModel());
        }

        [HttpPost, CustomerAuthentication]
        public async Task<ActionResult> EditPass(ResetPassModel model)
        {
            if (!ModelState.IsValid) return View(model);
            Customer enti = await CustomerDAO.GetByID((string)Session["Customer"]);
            if(enti.Password != model.OldPass)
            {
                ModelState.AddModelError("OldPass", "Password is incorrect");
                return View(model);
            }
            try
            {
                await CustomerDAO.ChangePass(enti.ID, model.NewPass);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError("OldPass", "Server can not change password");
                return View(model);
            }
        }

        public async Task<SelectList> ListRoom()
        {
            return new SelectList(await RoomDAO.GetAll(), "ID", "Name");
        }

    }
}