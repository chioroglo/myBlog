namespace Common.Dto.Paging.CursorPaging
{
    public class CursorPagedResult<T> where T: class
    {

        public int PageSize { get; set; }

        public int Total { get; set; }

        public int HeadElementId { get; set; }

        public int TailElementId { get; set; }

        public IList<T> Items { get; set; }
    }
}
