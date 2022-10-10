namespace Common.Models.Pagination
{
    public class PagedRequest
    {
        public PagedRequest()
        {
            RequestFilters = new RequestFilters();
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string ColumnNameForSorting { get; set; }

        public string SortingDirection { get; set; }

        public RequestFilters RequestFilters { get; set; }
    }
}
