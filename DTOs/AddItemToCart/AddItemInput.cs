namespace Foodtek.DTOs.AddItemToCart
{
    public class AddItemInput
    {

        public int UserId { get; set; }
        public int CartId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }

    }
}
