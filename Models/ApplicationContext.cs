using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class ApplicationContext: DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDrugs> SalesDrugs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDrugs> OrdersDrugs { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
