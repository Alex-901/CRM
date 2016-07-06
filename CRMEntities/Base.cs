using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEntities
{
    public class Base
    {
        [NotMapped]
        public List<ContactDetails> Contacts { get; set; } = new List<ContactDetails>();
        [NotMapped]
        public List<Agency> Agencies { get; set; } = new List<Agency>();
    }
}
