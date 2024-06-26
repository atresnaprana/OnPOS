﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace OnPOS.Models
{
    public class dbItemStore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string COMPANY_ID { get; set; }
        public int itemidint { get; set; }
        public string itemid { get; set; }
        public int storeid { get; set; }
        public string STORE_NAME { get; set; }
        public DateTime ENTRY_DATE { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public string FLAG_AKTIF { get; set; }
        [NotMapped]
        public List<dbItemMaster> itemList { get; set; }
        [NotMapped]
        public List<dbStoreList> StoreList { get; set; }
        [NotMapped]
        public List<string> itemidlist { get; set; }

        [NotMapped]
        public List<string> storeidlist { get; set; }
    }
}
