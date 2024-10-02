namespace WebApplication2.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalSum { get; set; }
        public DateTime SaleDate { get; set; }
        public List<OrderDrugs> OrdersDrugs { get; set; } = new List<OrderDrugs>();
    }
}
