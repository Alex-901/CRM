using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRMBusiness;
using CRMEntities;

namespace CRM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var agencies = new AgencyBusiness().LoadAgencys();

            var model = new Home
            {
                WelcomeMessage = $"Welcome to CRM. You have a total of {agencies.Count()} active agencies in the system."
            };

            return View(model);
        }
    }
}