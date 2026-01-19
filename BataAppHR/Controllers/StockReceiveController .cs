using BataAppHR.Data;
using Microsoft.AspNetCore.Mvc;
using OnPOS.Models;
using OnPOS.Models.DTOs;
using OnPOS.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnPOS.Controllers
{
    public class StockReceiveController : Controller
    {
        private readonly IStockService _stockService;
        private readonly FormDBContext db;

        public StockReceiveController(IStockService stockService, FormDBContext db)
        {
            _stockService = stockService;
            this.db = db;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Receive()
        {
            var model = new StockReceiveViewModel();
            return View(model);
        }
        // POST: /StockReceive/Receive
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(List<StockReceiveItemDto> scannedItems)
        {
            if (scannedItems == null)
            {
                // Handle error: no items submitted
                return RedirectToAction("Receive");
            }
            dbStoreList storedt = db.StoreListTbl.Where(y => y.STORE_EMAIL == User.Identity.Name).FirstOrDefault();

            // You would get the current user's store ID from their claims/session
            int currentUserStoreId = storedt.id;

            var (success, errorMessage) = await _stockService.ReceiveStockAsync(currentUserStoreId, scannedItems);

            if (success)
            {
                TempData["SuccessMessage"] = $"{scannedItems.Count} item(s) received successfully!";
                return RedirectToAction("Receive"); // Or wherever you want
            }
            else
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("Receive");
            }
        }
    }
}
