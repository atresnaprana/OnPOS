using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BataAppHR.Models
{
    public class dbNilaiSS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ScoreId { get; set; }
        public string SS_CODE { get; set; }
        public string EMP_TYPE { get; set; }

        public string TRN_ID { get; set; }
        public int NILAI { get; set; }
        public int SERTIFIKAT { get; set; }
        public string NoSertifikat { get; set; }
        public string FILE_SERTIFIKAT { get; set; }
        public DateTime Entry_Date { get; set; }
        public string Entry_User { get; set; }
        public DateTime Update_Date { get; set; }
        public string Update_User { get; set; }
        public string FLAG_AKTIF { get; set; }
        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public string Program { get; set; }
        [NotMapped]
        public string ProgramTxt { get; set; }
        [NotMapped]
        public string EDP { get; set; }
        [NotMapped]
        public string Periode { get; set; }
        [NotMapped]
        public string Week { get; set; }
        [NotMapped]
        public string Trainer { get; set; }
        [NotMapped]
        public string NAMA_SS { get; set; }
        [NotMapped]
        public string POSITION { get; set; }

    }
}
