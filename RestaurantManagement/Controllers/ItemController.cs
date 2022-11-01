using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Data;
using RestaurantManagement.Models;

namespace RestaurantManagement.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        // clear after submit
        static List<OrderItem> OItems = new List<OrderItem>();
        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var Items = new List<Item>();
            Items = _context.Items.ToList();

            ViewBag.orderedItems = OItems;
            return View(Items);           
        }

        public IActionResult addItem(string item, string delivery, int quantity)
        {

            var item_ = _context.Items
               .FirstOrDefault(m => m.ItemName == item);

            var newItem = new OrderItem(){
                OrderedItem = item,
                Quantity = quantity,
                UnitPrice = item_.Price,
            };

            OItems.Add(newItem);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveOrderedItem(string ItemName)
        {
            var ItemToRemove = OItems.Single(i => i.OrderedItem == ItemName);
            OItems.Remove(ItemToRemove);
            return RedirectToAction(nameof(Index));
        }

    }
}
