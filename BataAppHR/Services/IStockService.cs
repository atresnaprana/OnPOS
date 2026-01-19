using OnPOS.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnPOS.Services
{
    public interface IStockService
    {
        // The method takes a store ID and a list of items to be received.
        // It returns a tuple: a boolean for success and a string for any error message.
        Task<(bool Success, string ErrorMessage)> ReceiveStockAsync(int storeId, List<StockReceiveItemDto> itemsToReceive);
    }
}
