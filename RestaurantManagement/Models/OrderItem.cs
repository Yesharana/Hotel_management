namespace RestaurantManagement.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string OrderedItem { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}
