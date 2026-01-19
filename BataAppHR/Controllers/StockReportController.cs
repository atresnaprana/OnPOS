using BataAppHR.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace OnPOS.Controllers
{
    public class StockReportController : Controller
    {
        private readonly FormDBContext _context; // Replace 'YourDbContext' with the name of your actual DbContext

        public StockReportController(FormDBContext context)
        {
            _context = context;
        }

        // GET: /StockReport
        public async Task<IActionResult> Index()
        {
            // Fetch all stock items from the database.
            // AsNoTracking() is a performance optimization for read-only data.
            var stockList = await _context.StockTbl.AsNoTracking().OrderBy(s => s.storeid).ThenBy(s => s.itmname).ToListAsync();

            return View(stockList);
        }

        // GET: /StockReport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the specific stock item by its primary key (stock_id)
            var stockItem = await _context.StockTbl.FirstOrDefaultAsync(m => m.stock_id == id);

            if (stockItem == null)
            {
                return NotFound();
            }

            return View(stockItem);
        }
    }
}
