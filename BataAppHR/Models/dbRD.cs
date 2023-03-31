using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace BataAppHR.Models
{
    public class dbRD
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string RD_CODE { get; set; }
        public string EDP_CODE { get; set; }
        public string NM_RD { get; set; }
        public int FLAG_AKTIF { get; set; }
        public DateTime? RESIGN_DATE { get; set; }
        public string RESIGN_TXT { get; set; }
        public string SEX { get; set; }
        public string RD_HP { get; set; }
        public string RD_EMAIL { get; set; }
        public DateTime? JOIN_DATE { get; set; }
        public string RD_SERAGAM_SIZE { get; set; }
        public string RD_SEPATU_SIZEUK { get; set; }
        public string No_KTP { get; set; }
        public string VAKSIN1 { get; set; }
        public string VAKSIN2 { get; set; }
        public string FILE_SERTIFIKAT1 { get; set; }
        public string FILE_SERTIFIKAT2 { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
        public DateTime? ENTRY_DATE { get; set; }
        public string RD_PHOTO { get; set; }
        public string EMERGENCY_NAME { get; set; }
        public string EMERGENCY_PHONE { get; set; }
        public string EMERGENCY_ADDRESS { get; set; }
        public string RESIGN_TYPE { get; set; }
        public string RESIGN_TYPE2 { get; set; }
        public string FILE_MEDIC { get; set; }

        public int YEAR_LENGTH { get; set; }
        public int MONTH_LENGTH { get; set; }
        public int DAYS_LENGTH { get; set; }

        public string LAMA_KERJA { get; set; }


        [NotMapped]
        public List<XstoreModel> ddEdp { get; set; }
        [NotMapped]
        public List<string> EDP_CODE_LIST { get; set; }

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
        [NotMapped]
        public string STORE_NAME { get; set; }
        [NotMapped]
        public string STORE_UPDATE { get; set; }
        [NotMapped]
        public string STORE_UPDATE_PERSON { get; set; }

    }
}
