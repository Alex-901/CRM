using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CRMEntities
{
    public class Agency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        public Contact NewContact { get; set; } = new Contact();
    }
}
