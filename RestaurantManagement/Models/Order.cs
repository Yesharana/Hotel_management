namespace RestaurantManagement.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        //public int? TableNumber { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public string OrderType { get; set; }
        public string CustomerName { get; set; }
        public bool OrderStatus { get; set; }
        public bool PaymentStatus { get; set; }
    }
}
