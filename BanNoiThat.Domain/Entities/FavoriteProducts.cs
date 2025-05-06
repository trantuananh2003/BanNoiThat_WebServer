namespace BanNoiThat.Domain.Entities
{
    public class FavoriteProducts
    {
        public string Id { get; set; }
        public string Product_Id { get; set; }
        public Product Product { get; set; }
        public string User_Id { get; set; }
        public User User { get; set; }
    }
}
