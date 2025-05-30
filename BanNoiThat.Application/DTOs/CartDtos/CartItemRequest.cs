namespace BanNoiThat.Application.DTOs.CartDtos
{
    public class CartItemRequest
    {
        public string ProductItem_Id { get; set; }
        public int Quantity { get; set; }
        public bool IsAddManual { get; set; }
    }
}
