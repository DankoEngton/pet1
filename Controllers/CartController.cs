using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CartController : Controller
    {
        private Cart _cart;

        public CartController(Cart cart)
        {
            _cart = cart;
        }

        public IActionResult Index()
        {
            return View(_cart);
        }

        public IActionResult AddToCart(Drug drug, int quantity = 1)
        {
            var item = _cart.Items.FirstOrDefault(i => i.Drug.Id == drug.Id);

            if (item == null)
            {
                _cart.Items.Add(new CartItem { Drug = drug, Quantity = quantity });
            }
            else
            {
                item.Quantity += quantity;
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(Drug drug)
        {
            
            var item = _cart.Items.FirstOrDefault(i => i.Drug.Id == drug.Id);

            if (item != null)
            {
                _cart.Items.Remove(item);
            }

            return RedirectToAction("Index");
        }
    }
}
