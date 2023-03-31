using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BataAppHR.Models
{
    public class dbProgram
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ProgramId { get; set; }
        public string ProgramName { get; set; }
        public DateTime Entry_Date { get; set; }
        public string Entry_User { get; set; }
        public DateTime Update_Date { get; set; }
        public string Update_User { get; set; }
        public string FLAG_AKTIF { get; set; }

    }
}
