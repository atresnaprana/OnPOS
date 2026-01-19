using System.Collections.Generic;

namespace OnPOS.Models.DTOs
{
    public class StockReceiveApiRequest
    {
        public int StoreId { get; set; }
        public List<StockReceiveItemDto> Items { get; set; }
    }
}
