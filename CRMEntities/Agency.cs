using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CRMEntities
{
    public class Agency : Base
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Contact NewContact { get; set; } = new Contact();
        public List<HistoryItem> HistoryItems { get; set; } = new List<HistoryItem>();
    }
}
