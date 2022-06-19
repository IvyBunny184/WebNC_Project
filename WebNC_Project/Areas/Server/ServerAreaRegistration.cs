using System.Web.Mvc;

namespace WebNC_Project.Areas.Server
{
    public class ServerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Server";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Server_default",
                "Server/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "WebNC_Project.Areas.Server.Controllers" }
            );
        }
    }
}