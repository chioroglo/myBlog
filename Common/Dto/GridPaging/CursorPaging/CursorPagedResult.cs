namespace Common.Dto.GridPaging.CursorPaging
{
    public class CursorPagedResult<T> where T: class
    {
        public int PageIndex { get; set; }

        public int PageSoze { get; set; }

        public int Total { get; set; }

        public IList<T> Items { get; set; }

        public int HeadElementId { get; set; }

        public int TailElementId { get; set; }
    }
}
