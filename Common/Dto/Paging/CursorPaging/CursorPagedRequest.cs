namespace Common.Dto.Paging.CursorPaging
{
    public class CursorPagedRequest
    {
        public CursorPagedRequest()
        {
            RequestFilters = new RequestFilters();
        }

        public int PageSize { get; set; }

        public int? PivotElementId { get; set; }
        
        public bool GetNewer { get; set; }

        public RequestFilters RequestFilters { get; set; }
    }
}
