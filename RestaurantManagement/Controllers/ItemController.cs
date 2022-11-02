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

        public IActionResult addItem(string item, int quantity)
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

        public IActionResult RemoveOrderedItem(string ItemName, int ItemQua)
        {
            var ItemToRemove = OItems.First(i => i.OrderedItem == ItemName && i.Quantity == ItemQua);
            OItems.Remove(ItemToRemove);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddOrder(string customerName, string delivery)
        {
            //create Order
            //OrderItems, OrderType, CustomerName, OrderStatus, PaymentStatus
            var OStatus = false;
            if(delivery == "Dine In")
            {
                OStatus = true;
            }

            var newOrder = new Order() {
                OrderItems = OItems,
                OrderType = delivery,
                CustomerName = customerName,
                OrderStatus = OStatus,
                PaymentStatus = false,
            };

            // add & save Order to database
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            // empty the OItem list
            OItems.Clear();

            return RedirectToAction(nameof(Index));
        }

    }
}
