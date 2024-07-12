using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace OnPOS.Models
{
    public class dbDepartment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CodeDivisi { get; set; }
        public string DivisiName { get; set; }
        public string colorcode { get; set; }
        public string Color { get; set; }
        public string gendercode { get; set; }
        public string gender { get; set; }
        public string codemaincat { get; set; }
        public string maincat { get; set; }
        public string codesubcat { get; set; }
        public string subcat { get; set; }
        public DateTime ENTRY_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public string FLAG_AKTIF { get; set; }
        public string COMPANY_ID { get; set;  }

    }
}
