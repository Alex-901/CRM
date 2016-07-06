using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CRMEngine;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMEntities
{
    [Table("tb_ContactDetails")]
    public class ContactDetails
    {
        [Column("CD_Id")]
        [Key]
        public int ContactDetailId { get; set; } = 0;
        [Column("CD_Forname")]
        public string Forename { get; set; } = string.Empty;
        [Column("CD_Surname")]
        public string Surname { get; set; } = string.Empty;
        [Column("CD_Phone")]
        public int Mobile { get; set; } = 0;
        [Required]
        [Column("CD_Email")]
        public string Email { get; set; } = string.Empty;

        [NotMapped]
        public Constants.Enums.ContactDetailType ContactType { get; set; } = Constants.Enums.ContactDetailType.Agency;

        [NotMapped]
        public int EntityId { get; set; } = 0;
    }
}
