namespace namasdev.Core.Linq
{
    public class OrderAndPagingParameters
    {
        public string Order { get; set; }
        public string DefaultOrder { get; set; }
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public int ItemsTotalCount { get; set; }
    }
}
