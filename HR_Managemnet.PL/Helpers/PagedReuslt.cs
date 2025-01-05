namespace HR_Managemnet.PL.Helpers
{
    public class PagedReuslt<T> where T : class
    {
        public PagedReuslt()
        {

        }
        public IReadOnlyList<T> Data { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
