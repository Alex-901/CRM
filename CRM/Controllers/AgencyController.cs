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
            return View("CreateAgency");
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
                    return View("CreateAgency", model);
                }
            }
            catch
            {
                return View("CreateAgency", model);
            }
        }

        // GET: Agency/Edit/5
        public ActionResult Edit(int id)
        {
            return View("EditAgency", new AgencyBusiness().LoadAgency(id));
        }
        
        [HttpPost]
        public ActionResult Edit(Agency model)
        {
            if (ModelState.IsValid)
            {
                new AgencyBusiness().SaveAgency(model);

                return View("EditAgency", new AgencyBusiness().LoadAgency(model.Id));
            }
            return View("EditAgency", model);
        }

        [HttpPost]
        public ActionResult LoadContacts(int contactId, int agencyId)
        {
            var contacts = new AgencyBusiness().LoadContactDetails(CRMEngine.Constants.Enums.ContactDetailType.Agency, agencyId).Where(x => x.ContactDetailId == contactId).FirstOrDefault();

            return PartialView("~/Views/Shared/EditorTemplates/NewContact.cshtml", contacts);
        }

        // GET: Agency/Delete/5
        public ActionResult Delete(int id)
        {
            new AgencyBusiness().DeleteAgency(id);

            return View("Agencies", new AgencyBusiness().LoadAgencys());
        }

        [HttpPost]
        public ActionResult SaveContact(Contact contact)
        {
            new AgencyBusiness().SaveContactDetail(contact);

            return View("EditAgency", new AgencyBusiness().LoadAgency(contact.EntityId));
        }

        public ActionResult NewContact(Contact contact)
        {
            new AgencyBusiness().SaveContactDetail(contact);

            return PartialView("~/Views/Shared/EditorTemplates/ContactDetails.cshtml", contact);
        }
    }
}
