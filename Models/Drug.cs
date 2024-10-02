using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication2.Models
{
    public class Drug
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = new Category();
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = new Supplier();
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = new Warehouse();
        public List<OrderDrugs> OrdersDrugs { get; set; } = new List<OrderDrugs>();

    }
}
