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
            var historyItems = new AgencyBusiness().LoadHistoryItems(0, new DateTime(1800, 01, 01)).Count();

            var model = new Home
            {
                WelcomeMessage = $"You have a total of {historyItems} history items in the system."
            };

            return View(model);
        }
    }
}