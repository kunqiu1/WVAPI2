using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WVAPI2.Controllers
{
    public class Products
    {
        public int MyProperty { get; set; }
        public double MyProperty2 { get; set; }
    }
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();



        }
    }
}
