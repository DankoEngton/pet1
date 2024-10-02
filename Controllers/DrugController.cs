using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class DrugController : Controller
    {
        ApplicationContext db;

        public int SelectedCategoryId { get; set; }
        public List<Category> categories = new List<Category>();

        public DrugController(ApplicationContext context)
        {
            db = context;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await db.Drugs.Include(d =>d.Category).ToListAsync());
        }
        public IActionResult Create()
        {
            DrugViewModel drugViewModel = new DrugViewModel()
            {
                Categories = db.Categories.ToList(),
                Suppliers = db.Suppliers.ToList(),
                Warehouses = db.Warehouses.ToList()
            };

            List<SelectListItem> selectedCategories = drugViewModel.Categories
               .Select(c => new SelectListItem
               {
                   Value = c.Id.ToString(),
                   Text = c.Name
               })

               .ToList();
            List<SelectListItem> selectedSuppliers = drugViewModel.Suppliers
               .Select(c => new SelectListItem
               {
                   Value = c.Id.ToString(),
                   Text = c.Name
               })

               .ToList();
            List<SelectListItem> selectedWarehouses = drugViewModel.Warehouses
               .Select(c => new SelectListItem
               {
                   Value = c.Id.ToString(),
                   Text = c.Name
               })
               .ToList();

            drugViewModel.SelectedCategories = selectedCategories;
            drugViewModel.SelectedSuppliers = selectedSuppliers;
            drugViewModel.SelectedWarehouses = selectedWarehouses;

            return View(drugViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DrugViewModel drug)
        {
            decimal decimalValue = decimal.Parse(drug.Price);

            Drug drug1 = new Drug
            {
                Id= drug.Id,
                Name = drug.Name,
                Price = decimalValue,
                CategoryId = drug.CategoryId,
                Category = db.Categories.Find(drug.CategoryId),
                SupplierId= drug.SupplierId,
                Supplier = db.Suppliers.Find(drug.SupplierId),
                WarehouseId= drug.WarehouseId, 
                Warehouse = db.Warehouses.Find(drug.WarehouseId)
            };

            db.Drugs.Add(drug1);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public List<SelectListItem> ConvertToSelectListItems(List<Category> categories)
        {
            List<SelectListItem> selectListItems = categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            return selectListItems;
        }
    }
}
