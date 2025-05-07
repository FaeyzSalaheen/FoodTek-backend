namespace Foodtek.DTOs.AddItemToCart
{
    public class AddItemOutput
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int? CartId { get; set; }
    }
}
