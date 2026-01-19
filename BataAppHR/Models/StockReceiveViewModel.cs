using OnPOS.Models.DTOs;
using System.Collections.Generic;

namespace OnPOS.Models
{
    public class StockReceiveViewModel
    {
        public string ItemIdSku { get; set; }
        public int Quantity { get; set; } = 1;

        // This will hold the list of items scanned so far
        public List<StockReceiveItemDto> ScannedItems { get; set; } = new List<StockReceiveItemDto>();
    }
}
