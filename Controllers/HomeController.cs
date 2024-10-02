using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        [JsonIgnore]
        private readonly ISession _session;

        ApplicationContext db;

        public Cart _cart { get; set; }
        public Cart _orderCart { get; set; }

        public int SelectedCategoryId { get; set; }
        public List<Category> categories = new List<Category>();

        public HomeController(ApplicationContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
            db = context;
        }

        public async Task<IActionResult> Index(int fromPrice,string name, int categoryId, int toPrice=1000)
        {
            // Отримати список всіх продуктів
            var allDrugs = db.Drugs.ToList();

            // Отримати суму замовлених одиниць кожного продукту
            var orderedQuantities = db.OrdersDrugs
                .GroupBy(od => od.DrugId)
                .Select(g => new { DrugId = g.Key, OrderedQuantity = g.Sum(od => od.Quantity) })
                .ToDictionary(d => d.DrugId, d => d.OrderedQuantity);

            // Отримати суму проданих одиниць кожного продукту
            var soldQuantities = db.SalesDrugs
                .GroupBy(sd => sd.DrugId)
                .Select(g => new { DrugId = g.Key, SoldQuantity = g.Sum(sd => sd.Quantity) })
                .ToDictionary(d => d.DrugId, d => d.SoldQuantity);

            var model = new CategoryViewModel();
            model.Categories = db.Categories.ToList();
            List<SelectListItem> selectedCategories = model.Categories
               .Select(c => new SelectListItem
               {
                   Value = c.Id.ToString(),
                   Text = c.Name
               })
               .ToList();

            var quantitiesOnHand = new List<int>();

            // Оновити залишок кожного продукту на складі та додати до списку quantitiesOnHand
            foreach (var drug in allDrugs)
            {
                int orderedQuantity = orderedQuantities.ContainsKey(drug.Id) ? orderedQuantities[drug.Id] : 0;
                int soldQuantity = soldQuantities.ContainsKey(drug.Id) ? soldQuantities[drug.Id] : 0;
                int availableQuantity = orderedQuantity - soldQuantity;
                if (availableQuantity != null)
                {
                    quantitiesOnHand.Add(availableQuantity);
                }
                else quantitiesOnHand.Add(1);
            }

            model.SelectedCategories = selectedCategories;
        
            if (categoryId != null)
            {
                if (categoryId == 0)
                {
                    model.Drugs = db.Drugs.ToList();
                    model.QuantitiesOnHand = quantitiesOnHand;
                }
                else
                {
                    model.Drugs = db.Drugs.Where(d => d.CategoryId == categoryId).ToList();
                    model.SelectedCategoryId = categoryId;
                }
            }
            else
            {
                model.Drugs = db.Drugs.ToList();
                model.QuantitiesOnHand = quantitiesOnHand;
            }
            if (fromPrice != null)
                model.Drugs = model.Drugs.Where(d => d.Price >= fromPrice).ToList();
            else
            {
                model.Drugs = model.Drugs.ToList();
                model.QuantitiesOnHand = quantitiesOnHand;
            }
            if (toPrice != null)
                model.Drugs = model.Drugs.Where((d) => d.Price <= toPrice).ToList();
            else
            {
                model.Drugs = model.Drugs.ToList();
                model.QuantitiesOnHand = quantitiesOnHand;
            }


            return View(model);


            
        }
        public IActionResult Create()
        {
            DrugViewModel drugViewModel = new DrugViewModel()
            {
                Categories = db.Categories.ToList(),
                Suppliers = db.Suppliers.ToList(),
                Warehouses = db.Warehouses.ToList()
            };

            List<SelectListItem> selectedCategories = drugViewModel.Categories
               .Select(c => new SelectListItem
               {
                   Value = c.Id.ToString(),
                   Text = c.Name
               })

               .ToList();
            List<SelectListItem> selectedSuppliers = drugViewModel.Suppliers
               .Select(c => new SelectListItem
               {
                   Value = c.Id.ToString(),
                   Text = c.Name
               })

               .ToList();
            List<SelectListItem> selectedWarehouses = drugViewModel.Warehouses
               .Select(c => new SelectListItem
               {
                   Value = c.Id.ToString(),
                   Text = c.Name
               })
               .ToList();

            drugViewModel.SelectedCategories = selectedCategories;
            drugViewModel.SelectedSuppliers = selectedSuppliers;
            drugViewModel.SelectedWarehouses = selectedWarehouses;

            return View(drugViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DrugViewModel drug)
        {
            decimal decimalValue = decimal.Parse(drug.Price);

            Drug drug1 = new Drug
            {
                Id = drug.Id,
                Name = drug.Name,
                Price = decimalValue,
                CategoryId = drug.CategoryId,
                SupplierId = drug.SupplierId,
                WarehouseId = drug.WarehouseId
            };

            db.Drugs.Add(drug1);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public IActionResult AddToCart()
        {
            List<Drug> drugs = db.Drugs.Include(d => d.Category).ToList();
            string json = _session.GetString("cart");
            
            _cart = (Cart)JsonConvert.DeserializeObject<Cart>(json);
            if (_cart != null)
            {
                _cart.Drugs = drugs;
                return View(_cart);
            }
            else return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public IActionResult AddToCart(Drug drug, int quantity = 1)
        {
            Drug _drug;
            string json = _session.GetString("cart");
            _drug = db.Drugs.FirstOrDefault(r => r.Id == drug.Id); ;

            List<Drug> drugs = db.Drugs.Include(d => d.Category).ToList();
            Cart cart;

            if (json != null)
            {
                _cart = (Cart)JsonConvert.DeserializeObject<Cart>(json);
            }


            else 
            {
                _cart = new Cart() { Drugs = drugs };
            }
            _cart.Items.Add(new CartItem { Drug = _drug, Quantity = quantity });
            
            _session.SetString("cart", JsonConvert.SerializeObject(_cart));
            return RedirectToAction("AddToCart");
        }

        public IActionResult RemoveFromCart(Drug drug)
        {
            string json = _session.GetString("cart");

            _cart = (Cart)JsonConvert.DeserializeObject<Cart>(json);
            var item = _cart.Items.FirstOrDefault(i => i.Drug.Id == drug.Id);

            if (item != null)
            {
                _cart.Items.Remove(item);
            }
            _session.SetString("cart", JsonConvert.SerializeObject(_cart));
            return RedirectToAction("AddToCart");
        }

        public IActionResult CreateSale ()
        {
            string json = _session.GetString("cart");
            _cart = (Cart)JsonConvert.DeserializeObject<Cart>(json);
            decimal _sum = 0;

            List<Drug> drugs = new List<Drug>();
            foreach(CartItem d in _cart.Items)
            {
                var drug = db.Drugs.Find(d.Drug.Id);
                _sum += d.Drug.Price * d.Quantity;
                drugs.Add(d.Drug);
            }
            
            Sale sale = new Sale() { SaleDate=DateTime.Now, TotalSum= _sum};

            db.Sales.Add(sale);
            db.SaveChanges();

            SaleDrugs saleDrugs = new SaleDrugs();
            List<SaleDrugs> salesDrugs = new List<SaleDrugs>();
            foreach (CartItem d in _cart.Items)
            {
                salesDrugs.Add(new SaleDrugs()
                {
                    DrugId = d.Drug.Id,
                    SaleId = sale.Id,
                    Quantity = d.Quantity
                });
            }

            db.SalesDrugs.AddRange(salesDrugs);
            db.SaveChanges();

            _cart = null;
            _session.SetString("cart", JsonConvert.SerializeObject(_cart));
            return RedirectToAction("Index");
        }
        public List<SelectListItem> ConvertToSelectListItems(List<Category> categories)
        {
            List<SelectListItem> selectListItems = categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            
            return selectListItems;
        }

        [HttpGet]
        public IActionResult OpenCategory()
        {
            return RedirectToAction("Index","Category");
        }

        [HttpGet]
        public IActionResult OpenSupplier()
        {
            return RedirectToAction("Index", "Supplier");
        }

        [HttpGet]
        public IActionResult OpenWarehouse()
        {
            return RedirectToAction("Index", "Warehouse");
        }

        [HttpGet]
        public IActionResult OpenCart()
        {
            return RedirectToAction("AddToCart", "Home");
        }

        [HttpGet]
        public IActionResult AddToOrderCart()
        {
            List<Drug> drugs = db.Drugs.Include(d => d.Category).ToList();
            string json = _session.GetString("OrderCart");
            
            _orderCart = (Cart)JsonConvert.DeserializeObject<Cart>(json);
            if (_orderCart != null)
            {
                _orderCart.Drugs = drugs;
                return View(_orderCart);
            }
            else return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public IActionResult AddToOrderCart(Drug drug, int quantity = 1)
        {
            Drug _drug;
            string json = _session.GetString("OrderCart");
            _drug = db.Drugs.FirstOrDefault(r => r.Id == drug.Id); ;

            List<Drug> drugs = db.Drugs.Include(d => d.Category).ToList();
            Cart OrderCart;

            if (json != null)
            {
                _orderCart = (Cart)JsonConvert.DeserializeObject<Cart>(json);
            }


            else 
            {
                _orderCart = new Cart() { Drugs = drugs };
            }
            _orderCart.Items.Add(new CartItem { Drug = _drug, Quantity = quantity });
            
            _session.SetString("OrderCart", JsonConvert.SerializeObject(_orderCart));
            return RedirectToAction("AddToOrderCart");
        }

        public IActionResult RemoveFromOrderCart(Drug drug)
        {
            string json = _session.GetString("OrderCart");
            _orderCart = (Cart)JsonConvert.DeserializeObject<Cart>(json);
            var item = _orderCart.Items.FirstOrDefault(i => i.Drug.Id == drug.Id);
            if (item != null)
            {
                _orderCart.Items.Remove(item);
            }
            _session.SetString("OrderCart", JsonConvert.SerializeObject(_orderCart));
            return RedirectToAction("AddToOrderCart");
        }

        public IActionResult CreateOrder()
        {
            string json = _session.GetString("OrderCart");
            _orderCart = (Cart)JsonConvert.DeserializeObject<Cart>(json);
            decimal _sum = 0;

            List<Drug> drugs = new List<Drug>();
            foreach (CartItem d in _orderCart.Items)
            {
                var drug = db.Drugs.Find(d.Drug.Id);
                _sum += d.Drug.Price * d.Quantity;
                drugs.Add(d.Drug);
            }

            Order order = new Order() { SaleDate = DateTime.Now, TotalSum = _sum };

            db.Orders.Add(order);
            db.SaveChanges();

            OrderDrugs orderDrugs = new OrderDrugs();
            List<OrderDrugs> ordersDrugs = new List<OrderDrugs>();
            foreach (CartItem d in _orderCart.Items)
            {
                ordersDrugs.Add(new OrderDrugs()
                {
                    DrugId = d.Drug.Id,
                    OrderId = order.Id,
                    Quantity = d.Quantity
                });
            }

            db.OrdersDrugs.AddRange(ordersDrugs);
            db.SaveChanges();

            _orderCart = null;
            _session.SetString("OrderCart", JsonConvert.SerializeObject(_orderCart));
            return RedirectToAction("Index");
        }
    }
}