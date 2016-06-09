using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEntities;
using System.Data;
using System.Data.SqlClient;
using CRMEngine;

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

                using (var cmd = new SqlCommand("pSaveAgency", conn) { CommandType = CommandType.StoredProcedure })
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

                using (var cmd = new SqlCommand("pDeleteAgency", conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("Id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Agency> LoadAgencys()
        {
            var Agencys = new List<Agency>();

            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("pLoadAgencies", conn) { CommandType = CommandType.StoredProcedure })
                {
                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Agencys.Add(new Agency
                        {
                            Id = int.Parse(dr["AG_Id"].ToString()),
                            Name = dr["AG_Name"].ToString()
                        });
                    }
                }
            }

            return Agencys;
        }

        public void SaveContactDetail(Contact contact)
        {
            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("pSaveContactDetail", conn) { CommandType = CommandType.StoredProcedure })
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

        public List<Contact> LoadContactDetails(Constants.Enums.ContactDetailType type, int entityId)
        {
            var contacts = new List<Contact>();

            using (var conn = new SqlConnection(_connString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("pLoadContactDetail", conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("@LCD_Type", type.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@LCD_Entity_Id", entityId));

                    var dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        contacts.Add(new Contact
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
