using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BataAppHR.Models
{
    public class dbTrainerList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idTrainerList { get; set; }
        public string idTrainer { get; set; }
        public int idFormRekap { get; set; }
    }
}
