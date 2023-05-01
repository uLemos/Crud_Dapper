namespace Order.Domain.Models
{
    public class OrderItemModel : EntityBase
    {
        public OrderModel OrderId { get; set; }
        public ProductModel ProductId { get; set; }
        public decimal SellValue { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
