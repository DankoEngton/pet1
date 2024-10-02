namespace WebApplication2.Models
{
    public class SaleDrugs
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public Drug Drug { get; set; }
        public int SaleId { get; set; }
        public Sale Sale{ get; set; }
        public int Quantity { get; set; }
    }
}
