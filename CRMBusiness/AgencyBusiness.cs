using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CRMEntities;
using CRMData;
using System.Web;
using CRMEngine;

namespace CRMBusiness
{
    public class AgencyBusiness
    {
        public void SaveAgency(Agency agency)
        {
            var CRMData = new CRMData.CRMData(ConfigurationManager.ConnectionStrings["CRMConn"].ConnectionString);

            agency.Contacts.ForEach(x => SaveContactDetail(x));

            CRMData.SaveAgency(agency);
        }

        public void DeleteAgency(int id) => new CRMData.CRMData(ConfigurationManager.ConnectionStrings["CRMConn"].ConnectionString).DeleteAgency(id);

        public List<Agency> LoadAgencys() => new CRMData.CRMData(ConfigurationManager.ConnectionStrings["CRMConn"].ConnectionString).LoadAgencys();

        public void SaveContactDetail(Contact contact) => new CRMData.CRMData(ConfigurationManager.ConnectionStrings["CRMConn"].ConnectionString).SaveContactDetail(contact);

        public List<Contact> LoadContactDetails(Constants.Enums.ContactDetailType type, int entityId) => new CRMData.CRMData(ConfigurationManager.ConnectionStrings["CRMConn"].ConnectionString).LoadContactDetails(type, entityId);

        public Agency LoadAgency(int Id)
        {
            Agency agency = LoadAgencys().Where(x => x.Id == Id).FirstOrDefault();

            agency.Contacts = LoadContactDetails(CRMEngine.Constants.Enums.ContactDetailType.Agency, Id);

            return agency;
        }

        public void DeleteContactDetail(int contactId) => new CRMData.CRMData(ConfigurationManager.ConnectionStrings["CRMConn"].ConnectionString).DeleteContactDetail(contactId);

        public List<HistoryItem> LoadHistoryItems(int agencyId) => new CRMData.CRMData(ConfigurationManager.ConnectionStrings["CRMConn"].ConnectionString).LoadHistoryItems(agencyId);

        public void SaveHistoryItem(HistoryItem historyItem) => new CRMData.CRMData(ConfigurationManager.ConnectionStrings["CRMConn"].ConnectionString).SaveHistoryItem(historyItem);
    }
}
