namespace WebApplication2.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public List<Drug> Drugs { get; set; } = new List<Drug>();
    }
}
