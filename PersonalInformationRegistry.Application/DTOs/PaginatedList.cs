namespace PersonalInformationRegistry.Application.DTOs
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }

        public int TotalCount { get; set; }
        
        public int PageSize { get; set; }
        
        public int PageIndex { get; set; }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
