using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace CRMEntities
{
    [Table("tb_Agencies")]
    public class Agency : Base
    {
        [Column("AG_Id")]
        public int Id { get; set; }
        [Column("AG_Name")]
        public string Name { get; set; }
        [Column("AG_IsDeleted")]
        public bool Deleted { get; set; }
        [NotMapped]
        public ContactDetails NewContact { get; set; } = new ContactDetails();
        [NotMapped]
        public List<HistoryItem> HistoryItems { get; set; } = new List<HistoryItem>();

        public virtual ICollection<lt_ContactDetail> lt_ContactDetail { get; set; }
    }
}
