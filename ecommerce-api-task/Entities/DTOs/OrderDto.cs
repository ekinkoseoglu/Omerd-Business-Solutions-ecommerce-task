namespace ecommerce_api_task.Entities.DTOs
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
