using BataAppHR.Data;
using Microsoft.EntityFrameworkCore;
using OnPOS.Models;
using OnPOS.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnPOS.Services
{
    public class StockService : IStockService
    {
        private readonly FormDBContext _context; // Your EF Core DbContext

        public StockService(FormDBContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string ErrorMessage)> ReceiveStockAsync(int storeId, List<StockReceiveItemDto> itemsToReceive)
        {
            // Use a transaction to ensure all updates succeed or none do.
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in itemsToReceive)
                {
                    if (item.ItemIdSku.Length != 16)
                    {
                        // Basic validation
                        await transaction.RollbackAsync();
                        return (false, $"Invalid SKU length for {item.ItemIdSku}.");
                    }

                    // 1. Parse the SKU to get the base article and size
                    string baseItemId = item.ItemIdSku.Substring(0, 14);
                    string size = item.ItemIdSku.Substring(14, 2); // e.g., "40", "41"
                    string sizeColumnName = "s" + size;

                    // 2. Find the existing stock record for this item and store
                    var stockRecord = await _context.StockTbl.FirstOrDefaultAsync(s => s.storeid == storeId && s.itemid == baseItemId);

                    if (stockRecord == null)
                    {
                        // Item doesn't exist for this store, so create a new record
                        // First, verify the item exists in the master list
                        var itemMaster = await _context.ItemMasterTbl.FirstOrDefaultAsync(im => im.itemid == baseItemId);
                        if (itemMaster == null)
                        {
                            await transaction.RollbackAsync();
                            return (false, $"Item master data not found for article {baseItemId}.");
                        }

                        stockRecord = new dbStoreStockModel
                        {
                            storeid = storeId,
                            itemid = baseItemId,
                            itmname = itemMaster.itemdescription,
                            cat = itemMaster.category,
                            subcat = itemMaster.subcategory,
                            // Set all initial counts to 0
                            Past_stock = 0,
                            dispatch_qty = 0,
                            sales_qty = 0,
                            Current_stock = 0,
                            s33 = 0,
                            s34 = 0,
                            s35 = 0,
                            s36 = 0,
                            s37 = 0,
                            s38 = 0,
                            s39 = 0,
                            s40 = 0,
                            s41 = 0,
                            s42 = 0,
                            s43 = 0,
                            s44 = 0,
                            s45 = 0,
                            s46 = 0
                        };
                        _context.StockTbl.Add(stockRecord);
                    }

                    // 3. Update the quantities
                    stockRecord.receive_qty += item.Quantity;
                    stockRecord.Current_stock += item.Quantity;
                    stockRecord.lastrcvdate = DateTime.Now;

                    // 4. Use reflection to update the correct size column (s33, s34, etc.)
                    var sizeProperty = typeof(dbStoreStockModel).GetProperty(sizeColumnName);
                    if (sizeProperty != null && sizeProperty.CanWrite)
                    {
                        int currentSizeStock = (int)sizeProperty.GetValue(stockRecord);
                        sizeProperty.SetValue(stockRecord, currentSizeStock + item.Quantity);
                    }
                    else
                    {
                        // This item has an invalid size, roll back everything.
                        await transaction.RollbackAsync();
                        return (false, $"Invalid size '{size}' found for item {baseItemId}.");
                    }
                }

                // 5. Save all changes and commit the transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, null); // Success!
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception ex
                return (false, "An unexpected database error occurred.");
            }
        }
    }
}
