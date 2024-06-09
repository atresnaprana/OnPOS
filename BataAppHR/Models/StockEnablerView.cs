using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using OnPOS.Models;
using System.Collections.Generic;
using MimeDetective.Storage;
namespace OnPOS.Models
{
    public class StockEnablerView
    {
        public List<dbItemMaster> dataItems { get; set; }
        public List<dbStoreList> dataStore { get; set; }

    }  
}
