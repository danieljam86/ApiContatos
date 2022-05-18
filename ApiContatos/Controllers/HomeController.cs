using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApiContatos.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Sua aplicação.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Seu Contato.";

            return View();
        }
    }
}