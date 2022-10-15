namespace Common.Dto.GridPaging.CursorPaging
{
    public class CursorPagedRequest
    {
        public CursorPagedRequest()
        {

        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int HeadElementId { get; set; }

        public RequestFilters RequestFilters { get; set; }
    }
}
