using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class SupplierController : Controller
    {
        ApplicationContext db;
        public SupplierController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Suppliers.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            db.Suppliers.Add(supplier);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
