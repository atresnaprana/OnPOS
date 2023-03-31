using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace BataAppHR.Models
{
    public class VaksinModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string SS_CODE { get; set; }
        [Required]
        public string NAMA_SS { get; set; }
        public int FLAG_AKTIF { get; set; }
        public string SEX { get; set; }
        public string KTP { get; set; }
        public string HP_SS { get; set; }
        public string EMAIL_SS { get; set; }
        public string SIZE_SERAGAM { get; set; }
        public string SIZE_SEPATU_UK { get; set; }
        public DateTime? RESIGN_DATE { get; set; }
        public DateTime? JOIN_DATE { get; set; }
        public string RESIGN_TXT { get; set; }
        public string LAMA_KERJA { get; set; }
        public string VAKSIN1 { get; set; }
        public string VAKSIN2 { get; set; }
        public string FOTOSERTIFIKAT1 { get; set; }
        public string FOTOSERTIFIKAT2 { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public string STAFF_PHOTO { get; set; }
        public string EMERGENCY_NAME { get; set; }
        public string EMERGENCY_PHONE { get; set; }
        public string EMERGENCY_ADDRESS { get; set; }
        public string RESIGN_TYPE { get; set; }
        public string RESIGN_TYPE2 { get; set; }
        public string POSITION { get; set; }
        public string FILE_MEDIC { get; set; }
        public int YEAR_LENGTH { get; set; }
        public int MONTH_LENGTH { get; set; }
        public int DAYS_LENGTH { get; set; }
        //[Required]
        public string XSTORE_LOGIN { get; set; }

        public DateTime? UPDATE_DATE { get; set; }
        public DateTime? ENTRY_DATE { get; set; }
        [NotMapped]
        public List<XstoreModel> ddEdp { get; set; }
        [Required]
        public string EDP_CODE { get; set; }
        [NotMapped]
        public bool VAKSIN1Bool { get; set; }
        [NotMapped]
        public bool VAKSIN2Bool { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile fileVaksin1 { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile fileVaksin2 { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile fileMedic { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile filePhoto { get; set; }

    }
}
