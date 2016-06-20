using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEntities
{
    public class Base
    {
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        public List<Agency> Agencies { get; set; } = new List<Agency>();
    }
}
