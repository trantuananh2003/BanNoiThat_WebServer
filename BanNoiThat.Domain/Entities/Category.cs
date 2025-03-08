namespace BanNoiThat.Domain.Entities
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Parent_Id { get; set; }
        public Category Parent { get; set; }
        public string? CategoryUrlImage { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
