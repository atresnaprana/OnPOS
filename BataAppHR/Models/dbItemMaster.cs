
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace OnPOS.Models
{
    public class dbItemMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string COMPANY_ID { get; set; }
        public string itemid { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string itemdescription { get; set; }
        

        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal price1 { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal price2 { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal price3 { get; set; }

        public int? month_age { get; set; }
        public int? year_age { get; set; }
        public int? month_qty { get; set; }
        public int? month_amount { get; set; }
        public int? CY_qty { get; set; }
        public int? CY_amount { get; set; }
        public int? LY_qty { get; set; }
        public int? LY_amount { get; set; }
        public string gender { get; set; }


        public string brand { get; set; }

        public DateTime ENTRY_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public string FLAG_AKTIF { get; set; }
        public string codedivisi { get; set; }

        [NotMapped]
        public List<DropDownModel> ddgender { get; set; }
        [NotMapped]
        public int discperc { get; set; }
        [NotMapped]
        public int discamt { get; set; }
        [NotMapped]
        public string disctype { get; set; }

        [NotMapped]
        public List<dbCategory> ddcat { get; set; }
        
        [NotMapped]
        public List<dbSubCategory> ddsubcat { get; set; }
        
        [NotMapped]
        public string syserr { get; set; }
        [NotMapped]
        public List<dbStoreList> StoreList { get; set; }
        [NotMapped]
        public List<dbItemStore> itemSettings { get; set; }
        [NotMapped]
        public List<DropDownModel> ddDepartment { get; set; }

    }
}
