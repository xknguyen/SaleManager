using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;

namespace SaleManager.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}