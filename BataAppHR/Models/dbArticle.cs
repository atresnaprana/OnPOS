using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace BataAppHR.Models
{
    public class dbArticle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string Article { get; set; }
        public byte[] FILE_IMG { get; set; }
        public string FILE_IMG_NAME { get; set; }
        public DateTime ENTRY_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public string FLAG_AKTIF { get; set; }
        [NotMapped]
        public List<Article> listArticle { get; set; }
        [NotMapped]
        public string pricestring { get; set; }
        [NotMapped]
        public string projectname { get; set; }
        [NotMapped]
        public string isAvail { get; set; }
        [NotMapped]
        public string qty { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile fileImg { get; set; }

    }
}
