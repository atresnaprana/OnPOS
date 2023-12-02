using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using OnPOS.Models;
using System.Collections.Generic;
using MimeDetective.Storage;

namespace OnPOS.Models
{
    public class dbSubCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string COMPANY_ID { get; set; }
        [Required]
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string description { get; set; }
        public DateTime ENTRY_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public string FLAG_AKTIF { get; set; }
        [NotMapped]
        public List<dbCategory> ddcat { get; set; }
    }
}
