using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace BataAppHR.Models
{
    public class articlefront
    {
        public List<dbArticle> dataarticle { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile fileUploadArticle { get; set; }
        public string error { get; set; }
        public bool isPassed { get; set; }
    }
}
