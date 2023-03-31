using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BataAppHR.Models
{
    public class XstoreModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string edp { get; set; }
        public string district { get; set; }
        public string store_location { get; set; }
        public string area { get; set; }
        public string inactive_flag { get; set; }
        public string genesis_Flag { get; set; }
        public string RD_CODE { get; set; }
        public string STORE_CONCEPT { get; set; }
        public decimal? SELLING_AREA { get; set; }
        public decimal? STOCK_AREA { get; set; }
        public decimal? TOTAL_AREA { get; set; }
        public string ADDRESS { get; set; }
        public DateTime? OPENING_DATE { get; set; }

        public DateTime? LEASE_START { get; set; }
        public DateTime? LEASE_EXPIRED { get; set; }
        public DateTime? LAST_RENOVATION_DATE { get; set; }

        public string FILE_LEASE { get; set; }
        public string STORE_IMAGE { get; set; }
        public decimal? GROSS_VAL { get; set; }
        public decimal? SELLING_VAL { get; set; }
        public decimal? STORAGE_VAL { get; set; }
        public string FILE_OTHERS { get; set; }
        public byte[] STORE_IMG_BLOB { get; set; }
        public int? FLAG_APPROVAL { get; set; }

        public DateTime? CLOSE_DATE { get; set; }
        public string IS_DS { get; set; }
        public double RD_COMM { get; set; }
        public double DEPT_COMM { get; set; }
        public string IS_PERC { get; set; }
        public decimal? RD_COMM_PERC { get; set; }
        public decimal? DEPT_COMM_PERC { get; set; }
        public string IS_PERC_DEPT { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
        public DateTime? ENTRY_DATE { get; set; }


        [NotMapped]
        public string RD_COMM_PERC_STRING { get; set; }
        [NotMapped]
        public string DEPT_COMM_PERC_STRING { get; set; }
        [NotMapped]
        public string SELLING_AREA_STRING { get; set; }
        [NotMapped]
        public string STOCK_AREA_STRING { get; set; }
        [NotMapped]
        public string TOTAL_AREA_STRING { get; set; }
        [NotMapped]
        public string GROSS_VAL_STRING { get; set; }
        [NotMapped]
        public string STORAGE_VAL_STRING { get; set; }
        [NotMapped]
        public string SELLING_VAL_STRING { get; set; }
        [NotMapped]
        public bool genesis_FlagBool { get; set; }
        [NotMapped]
        public bool IS_DS_BOOL { get; set; }
        [NotMapped]
        public bool IS_PERC_BOOL { get; set; }
        [NotMapped]
        public bool IS_PERC_DEPT_BOOL { get; set; }
        [NotMapped]
        public bool inactive_flagBool { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile fileToko { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile LeaseFile { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
      
        public IFormFile FileOthers { get; set; }
        [NotMapped]
        public List<dbRD> rdlist { get; set; }
        [NotMapped]
        public List<XstoreModel> ddEdp { get; set; }
    }
}
