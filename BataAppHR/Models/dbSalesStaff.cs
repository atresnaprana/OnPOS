using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace OnPOS.Models
{
    public class dbSalesStaff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string COMPANY_ID { get; set; }
        public int STORE_ID { get; set; }
        public string SALES_NAME { get; set; }
        public DateTime SALES_REG_DATE { get; set; }
        public DateTime ENTRY_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public string FLAG_AKTIF { get; set; }
        public byte[] FILE_PHOTO { get; set; }
        public string FILE_PHOTO_NAME { get; set; }
        public string SALES_PHONE { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string SALES_EMAIL { get; set; }
        public string SALES_KTP { get; set; }
        public string SALES_BLACKLIST_FLAG { get; set; }

        [NotMapped]
        public bool isBlackList { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile filePhoto { get; set; }
        [NotMapped]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [NotMapped]
        public string errmsg { get; set; }
      
    }
}
