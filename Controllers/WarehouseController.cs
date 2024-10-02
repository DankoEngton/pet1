using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class WarehouseController : Controller
    {
        ApplicationContext db;
        public WarehouseController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Warehouses.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Warehouse warehouse)
        {

            db.Warehouses.Add(warehouse);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
