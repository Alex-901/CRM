using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRMEntities;
using CRMBusiness;

namespace CRM.Controllers
{
    public class AgencyController : Controller
    {
        // GET: Agency
        public ActionResult Index()
        {
            try
            {
                return View("Agencies", new AgencyBusiness().LoadAgencys());
            }
            catch (Exception)
            {
                return View();
            }
        }
        
        // GET: Agency/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agency/Create
        [HttpPost]
        public ActionResult Create(Agency model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mod = new List<Agency>();
                    var AgencyBusiness = new AgencyBusiness();
                    var service = new CRM.CRMService.CRMServiceClient();

                    //string agencyStatus = string.Empty;

                    //if (service != null)
                    //    agencyStatus = service.GetAgencyStatus(0);

                    Agency Agency = new Agency
                    {
                        Name = model.Name
                    };

                    AgencyBusiness.SaveAgency(Agency);

                    mod.Add(Agency);

                    return View("Agencies", new AgencyBusiness().LoadAgencys());
                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Agency/Edit/5
        public ActionResult View(int id)
        {
            return View("View", new AgencyBusiness().LoadAgency(id));
        }

        [HttpPost]
        public ActionResult View(Agency model)
        {
            if (ModelState.IsValid)
            {
                var agencyBusiness = new AgencyBusiness();

                agencyBusiness.SaveAgency(model);
            }

            return View("View", model);
        }

        [HttpPost]
        public ActionResult LoadContact(int contactId, int agencyId)
        {
            var contact = new Contact();

            if (contactId > 0)
            {
                contact = new AgencyBusiness().LoadContactDetails(CRMEngine.Constants.Enums.ContactDetailType.Agency, agencyId)
                    .Where(x => x.ContactDetailId == contactId).FirstOrDefault();
            }

            return PartialView("~/Views/Shared/EditorTemplates/NewContact.cshtml", contact);
        }

        [HttpPost]
        public ActionResult LoadContacts(int agencyId)
        {
            var contacts = new AgencyBusiness().LoadContactDetails(CRMEngine.Constants.Enums.ContactDetailType.Agency, agencyId);
               
            return PartialView("~/Views/Shared/EditorTemplates/ContactDetails.cshtml", contacts);
        }

        [HttpPost]
        public JsonResult LoadContactsByAgency(int agencyId)
        {
            var contacts = new AgencyBusiness().LoadContactDetails(CRMEngine.Constants.Enums.ContactDetailType.Agency, agencyId);

            return Json(contacts);
        }

        // GET: Agency/Delete/5
        public ActionResult Delete(int id)
        {
            new AgencyBusiness().DeleteAgency(id);

            return View("Agencies", new AgencyBusiness().LoadAgencys());
        }

        public ActionResult SaveContact(Contact contact)
        {
            var agencyBusiness = new AgencyBusiness();

            agencyBusiness.SaveContactDetail(contact);

            var contacts = agencyBusiness.LoadContactDetails(CRMEngine.Constants.Enums.ContactDetailType.Agency, contact.EntityId);

            return PartialView("~/Views/Shared/EditorTemplates/ContactDetails.cshtml", contacts);
        }
       
        public ActionResult DeleteContact(int contactId, int agencyId)
        {
            var agencyBusiness = new AgencyBusiness();

            agencyBusiness.DeleteContactDetail(contactId);

            var contacts = agencyBusiness.LoadContactDetails(CRMEngine.Constants.Enums.ContactDetailType.Agency, agencyId);

            return PartialView("~/Views/Shared/EditorTemplates/ContactDetails.cshtml", contacts);
        }
        
        public ActionResult LoadHistoryItems(int agencyId)
        {
            var agencyBusiness = new AgencyBusiness();

            var historyItems = agencyBusiness.LoadHistoryItems(agencyId, new DateTime(1800, 01, 01));

            return PartialView("~/Views/Shared/EditorTemplates/HistoryItems.cshtml", historyItems);
        }

        [HttpPost]
        public ActionResult AddHistoryItem(int agencyId)
        {
            var historyItem = new HistoryItem();

            historyItem.Agencies = new CRMBusiness.AgencyBusiness().LoadAgencys();

            if (agencyId > 0)
            {
                historyItem.Contacts = new CRMBusiness.AgencyBusiness().LoadContactDetails(CRMEngine.Constants.Enums.ContactDetailType.Agency, agencyId);
            }

            return PartialView("~/Views/Shared/EditorTemplates/HistoryItem.cshtml", historyItem);
        }

        public JsonResult LoadCalendarHistoryItems(int agencyId, DateTime date)
        {
            var agencyBusiness = new AgencyBusiness();

            var historyItems = agencyBusiness.LoadHistoryItems(agencyId, date);

            return Json(historyItems);
        }

        [HttpPost]
        public ActionResult SaveHistoryItem(HistoryItem historyItem)
        {
            var agencyBusiness = new AgencyBusiness();
            
            agencyBusiness.SaveHistoryItem(historyItem);

            var historyItems = agencyBusiness.LoadHistoryItems(historyItem.ParentId, new DateTime(1800, 01, 01));

            return PartialView("~/Views/Shared/EditorTemplates/HistoryItems.cshtml", historyItems);
        }
    }
}
