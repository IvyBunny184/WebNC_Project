using Firebase.Auth;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebNC_Project.Areas.Server.Controllers
{
    [ServerAuthentication]
    public class HomeController : Controller
    {
        // GET: Server/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }
    }
}