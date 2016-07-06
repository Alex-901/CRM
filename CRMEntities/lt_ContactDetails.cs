using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEntities
{
    public class lt_ContactDetail
    {
        [Key]
        public int LCD_Id { get; set; }
        public string LCD_Type { get; set; }
        public int LCD_Entity_Id { get; set; }
        public int LCD_CD_Id { get; set; }
        public bool LCD_Deleted { get; set; }

        [ForeignKey("LCD_Entity_Id")]
        public virtual Agency tb_Agencies { get; set; }
        [ForeignKey("LCD_CD_Id")]
        public virtual ContactDetails tb_ContactDetails { get; set; }
    }
}
