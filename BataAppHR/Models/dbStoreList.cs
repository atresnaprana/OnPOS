using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;


namespace OnPOS.Models
{
    public class dbStoreList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string COMPANY_ID { get; set; }
        public string STORE_NAME { get; set; }
        public string STORE_ADDRESS { get; set; }
        public string STORE_CITY { get; set; }
        public string STORE_PROVINCE { get; set; }
        public string STORE_POSTAL { get; set; }
        public string STORE_BANK_NAME { get; set; }
        public string STORE_BANK_NUMBER { get; set; }
        public string STORE_BANK_BRANCH { get; set; }
        public string STORE_BANK_COUNTRY { get; set; }
        public DateTime STORE_REG_DATE { get; set; }
        public string STORE_BL_FLAG { get; set; }
        public DateTime ENTRY_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public string FLAG_AKTIF { get; set; }
        public byte[] FILE_PHOTO { get; set; }

        public string FILE_PHOTO_NAME { get; set; }
        public string STORE_MANAGER_NAME { get; set; }
        public string STORE_MANAGER_PHONE { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string STORE_MANAGER_EMAIL { get; set; }
        public string STORE_MANAGER_KTP { get; set; }
        public string STORE_EMAIL { get; set; }

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
