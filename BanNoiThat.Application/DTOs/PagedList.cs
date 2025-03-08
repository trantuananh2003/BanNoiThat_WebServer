namespace BanNoiThat.Application.DTOs
{
    public class PagedList<T> 
    {
        public List<T>? Items { set; get; }
        public int PageCurrent { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }

        public PagedList(List<T> items, int pageCurrent, int pageSize, int totalCount)
        {
            Items = items;
            PageCurrent = pageCurrent;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double) pageSize);
        }
    }
}
