namespace BanNoiThat.Application.DTOs
{
    public class CategoryResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? CategoriesUrlImage { get; set; }
        public string? Parent_Id { get; set; }
        public List<CategoryResponse> Children { get; set; } = new List<CategoryResponse>();
    }
}
