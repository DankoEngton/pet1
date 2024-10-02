namespace WebApplication2.Models
{
    public class OrderDrugs
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public Drug Drug { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
