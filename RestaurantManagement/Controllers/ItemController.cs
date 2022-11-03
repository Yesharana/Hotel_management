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
        static bool isEmptyOrderList = false;
        static Order orderDetails = new Order();
        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var Items = new List<Item>();
            Items = _context.Items.ToList();

            ViewBag.orderedItems = OItems;
            ViewBag.isEmptyOrderList = isEmptyOrderList;
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

            isEmptyOrderList = false;
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
                PaymentStatus = OStatus,
                OrderDate = DateTime.Now,
            };

            // add & save Order to database
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            // empty the OItem list
            
            isEmptyOrderList = false;
            orderDetails = newOrder;

            return RedirectToAction(nameof(ViewBill));
             
        }

        public IActionResult ViewAllOrders()
        {
            var AllOrders = _context.Orders.ToList();
            return View(AllOrders);
        }

        public IActionResult ViewPendingOrders()
        {
            var pendingOrders = _context.Orders.ToList().FindAll(m => m.OrderStatus == false);
            return View(pendingOrders);
        }

        public IActionResult changeOrderStatus(int orderId)
        {
            var order = _context.Orders.First(m => m.OrderId == orderId);
            order.OrderStatus = true;
            _context.Orders.Update(order);
            _context.SaveChanges();

            return RedirectToAction(nameof(ViewPendingOrders));
        }


        public IActionResult ViewPendingPayments()
        {
            var pendingPayments = _context.Orders.ToList().FindAll(m => m.PaymentStatus == false);
            return View(pendingPayments);
        }

        public IActionResult changePaymentStatus(int orderId)
        {
            var order = _context.Orders.First(m => m.OrderId == orderId);
            order.PaymentStatus = true;
            _context.Orders.Update(order);
            _context.SaveChanges();

            return RedirectToAction(nameof(ViewPendingPayments));
        }

        public IActionResult ViewBill()
        {
            // check for the empty OList
            if (OItems.Count == 0)
            {
                isEmptyOrderList = true;
                return RedirectToAction(nameof(Index));
               
            }

            Order ord = orderDetails;
            ViewBag.ordItems = ord.OrderItems.ToList();
            //orderDetails = null;
            //OItems.Clear();
            return View(ord);
        }
    }
}   
