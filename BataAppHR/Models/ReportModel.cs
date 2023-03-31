using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataAppHR.Models
{
    public class ReportModel
    {
        public int ID { get; set; }
        public string DISTRICT { get; set; }
        public string EDP_CODE { get; set; }
        public string STORE_LOCATION { get; set; }
        public string AREA { get; set; }
        public string SS_CODE { get; set; }
        public string NAMA_SS { get; set; }
        public string POSITION { get; set; }
        public int FLAG_AKTIF { get; set; }
        public string SEX { get; set; }
        public string KTP { get; set; }
        public string HP_SS { get; set; }
        public string EMAIL_SS { get; set; }
        public string SIZE_SERAGAM { get; set; }
        public string SIZE_SEPATU_UK { get; set; }
        public DateTime? JOIN_DATE { get; set; }
        public DateTime? RESIGN_DATE { get; set; }
        public string RESIGN_TXT { get; set; }
        public string RESIGN_TYPE { get; set; }
        public string RESIGN_TYPE2 { get; set; }
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
    
        public int YEAR_LENGTH { get; set; }
        public int MONTH_LENGTH { get; set; }
        public int DAYS_LENGTH { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
        public DateTime? ENTRY_DATE { get; set; }
        public DateTime? STORE_UPDATE { get; set; }
        public string STORE_UPDATE_PERSON { get; set; }


    }
}
