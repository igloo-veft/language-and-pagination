namespace CoursesAPI.Models 
{
    public class Paging
    {
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalNumberOfItems { get; set; }
    }
}