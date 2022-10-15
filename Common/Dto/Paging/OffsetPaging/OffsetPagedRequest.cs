namespace Common.Dto.Paging.OffsetPaging
{
    public class OffsetPagedRequest
    {
        public OffsetPagedRequest()
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
