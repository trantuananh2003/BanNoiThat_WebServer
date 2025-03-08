namespace BanNoiThat.API.Model
{
    public class PaginationDto
    {
        public int CurrentPage { set; get; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
}
