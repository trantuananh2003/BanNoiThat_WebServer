namespace BanNoiThat.Domain.Entities
{
    public class Cart
    {
        public string Id { get; set;}
        public string User_Id { get; set; }
        //Need object User
        public List<CartItem> CartItems { get; set; }
    }

    public class CartItem
    {
        public string Id { get; set; }
        public string ProductItem_Id { get; set; }
        public ProductItem ProductItem { get; set; }
        public string Cart_Id { get; set; }
        public Cart Cart { get; set; }
        public int Quantity { get; set; }
    }
}
