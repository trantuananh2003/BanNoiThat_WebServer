namespace BanNoiThat.Application.DTOs
{
    public class CartItemRequest
    {
        public string ProductItem_Id { get; set; }
        public int Quantity { get; set; }
        public bool IsAddManual { get; set; }
    }
}
