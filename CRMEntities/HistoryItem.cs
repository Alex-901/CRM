using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEntities
{
    public class HistoryItem : Base
    {
        public int Id { get; set; }
        public ContactDetails ContactDetails { get; set; } = new ContactDetails();
        [Required]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = new DateTime(1800, 01, 01);
        public int ParentId { get; set; }
        public Agency Agency { get; set; } = new Agency();
    }
}
