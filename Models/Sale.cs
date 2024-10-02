namespace WebApplication2.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public decimal TotalSum { get; set; }
        public DateTime SaleDate { get; set; }
        public List<SaleDrugs> SaleDrugs { get; set; } = new List<SaleDrugs>();
    }
}
