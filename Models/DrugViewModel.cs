using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication2.Models
{
    public class DrugViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; }
        public int SupplierId { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public int WarehouseId { get; set; }
        public List<Warehouse> Warehouses { get; set; }

        public List<SelectListItem> SelectedCategories { get; set; }
        public List<SelectListItem> SelectedSuppliers { get; set; }
        public List<SelectListItem> SelectedWarehouses { get; set; }
    }
}
