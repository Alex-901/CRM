using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEntities;
using System.Data;
using System.Data.SqlClient;
using CRMEngine;
using System.Data.Entity;

namespace CRMData
{
    public class CRMData
    {
        string _connString;

        public CRMData(string connString)
        {
            _connString = connString;
        }

        public void SaveAgency(Agency agency)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(Constants.StoredProcedures.SaveAgency, conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("Id", agency.Id));
                    cmd.Parameters.Add(new SqlParameter("Name", agency.Name));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteAgency(int id)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(Constants.StoredProcedures.DeleteAgency, conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("Id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Agency> LoadAgencys()
        {
            using (var ctx = new DBContext())
            {
                return new List<Agency>(ctx.tb_Agencies
                    .Where(x => !x.Deleted)
                    .Include(y => y.lt_ContactDetail
                    .Select(z => z.tb_ContactDetails))
                    .ToList());
            }
        }

        public List<HistoryItem> LoadHistoryItems(int agency_Id, DateTime date)
        {
            var historyItems = new List<HistoryItem>();

            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(Constants.StoredProcedures.LoadHistoryItems, conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("@AG_Id", agency_Id));
                    cmd.Parameters.Add(new SqlParameter("@Date", date));

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        historyItems.Add(new HistoryItem
                        {
                            Id = int.Parse(dr["HI_Id"].ToString()),
                            CreationDate = DateTime.Parse(dr["HI_CreationDate"].ToString()),
                            Notes = dr["HI_Note"].ToString(),
                            ContactDetails = new ContactDetails
                            {
                                ContactDetailId = int.Parse(dr["CD_Id"].ToString()),
                                Forename = dr["CD_Forname"].ToString(),
                                Surname = dr["CD_Surname"].ToString()
                            },
                            Agency = new Agency
                            {
                                Name = dr["AG_Name"].ToString()
                            }
                        });
                    }
                }
            }

            return historyItems;
        }

        public void SaveHistoryItem(HistoryItem historyItem)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(Constants.StoredProcedures.SaveHistoryItems, conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("@AG_Id", historyItem.ParentId));
                    cmd.Parameters.Add(new SqlParameter("@HI_Id", historyItem.Id));
                    cmd.Parameters.Add(new SqlParameter("@CD_Id", historyItem.ContactDetails.ContactDetailId));
                    cmd.Parameters.Add(new SqlParameter("@HI_Note", historyItem.Notes));
                    cmd.Parameters.Add(new SqlParameter("@HI_CreationDate", historyItem.CreationDate));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteContactDetail(int contactId)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(Constants.StoredProcedures.DeleteContactDetail, conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("@CD_Id", contactId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveContactDetail(ContactDetails contact)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(Constants.StoredProcedures.SaveContactDetail, conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("@LCD_Type", contact.ContactType.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@LCD_Entity_Id", contact.EntityId));
                    cmd.Parameters.Add(new SqlParameter("@CD_Forname", contact.Forename));
                    cmd.Parameters.Add(new SqlParameter("@CD_Surname", contact.Surname));
                    cmd.Parameters.Add(new SqlParameter("@CD_Phone", contact.Mobile));
                    cmd.Parameters.Add(new SqlParameter("@CD_Email", contact.Email));
                    cmd.Parameters.Add(new SqlParameter("@CD_Id", contact.ContactDetailId));
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<ContactDetails> LoadContactDetails(Constants.Enums.ContactDetailType type, int entityId)
        {
            var contacts = new List<ContactDetails>();

            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(Constants.StoredProcedures.LoadContactDetail, conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("@LCD_Type", type.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@LCD_Entity_Id", entityId));

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        contacts.Add(new ContactDetails
                        {
                            ContactDetailId = int.Parse(dr["LCD_CD_Id"].ToString()),
                            EntityId = int.Parse(dr["LCD_Entity_Id"].ToString()),
                            Forename = dr["CD_Forname"].ToString(),
                            Surname = dr["CD_Surname"].ToString(),
                            Mobile = int.Parse(dr["CD_Phone"].ToString()),
                            Email = dr["CD_Email"].ToString()
                        });
                    }
                }
            }

            return contacts;
        }
    }
}
