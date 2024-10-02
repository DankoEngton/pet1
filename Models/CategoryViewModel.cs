using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication2.Models
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; }
        public List<SelectListItem> SelectedCategories { get; set; }
        public int? SelectedCategoryId { get; set; }
        public List<Drug> Drugs { get; set; }
        public List<int> QuantitiesOnHand { get; set; }
        public int FromPrice { get; set; }
        public int ToPrice { get; set; }
    }
}
