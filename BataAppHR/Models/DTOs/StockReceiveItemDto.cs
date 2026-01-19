namespace OnPOS.Models.DTOs
{
    public class StockReceiveItemDto
    {
        public string ItemIdSku { get; set; } // The full 16-digit SKU (e.g., 80966831615047)
        public int Quantity { get; set; }
    }
}
