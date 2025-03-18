namespace BanNoiThat.Domain.Entities
{
    public class ProductConfig
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Product_Id { get; set; }
        public Product Product { get; set; }
    }
}
