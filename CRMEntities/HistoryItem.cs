using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEntities
{
    public class HistoryItem
    {
        public int Id { get; set; }
        public Contact Contact { get; set; } = new Contact();
        [Required]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = new DateTime();
        public int ParentId { get; set; }
        [Required]
        public List<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
