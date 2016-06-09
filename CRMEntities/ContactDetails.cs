using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CRMEngine;

namespace CRMEntities
{
    public abstract class ContactDetails
    {
        public int ContactDetailId { get; set; } = 0;
        public string Forename { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public int Mobile { get; set; } = 0;
        [Required]
        public string Email { get; set; } = string.Empty;

        public Constants.Enums.ContactDetailType ContactType { get; set; } = Constants.Enums.ContactDetailType.Agency;

        public int EntityId { get; set; } = 0;
    }
}
