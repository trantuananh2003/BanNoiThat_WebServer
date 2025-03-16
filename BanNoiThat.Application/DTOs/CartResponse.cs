namespace BanNoiThat.Application.DTOs
{
    public class CartResponse
    {
        public string Id { get; set; }
        //Need object User
        public List<CartItemResponse> CartItems { get; set; }
    }

    public class CartItemResponse
    {
        public string Id { get; set; }
        public string ProductItem_Id { get; set; }
        public string ProductName { get; set; }
        public string NameOption { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
    }
}
