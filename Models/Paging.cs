namespace CoursesAPI.Models 
{
    public class Paging
    {
        /// <summary>
		/// The number of pages.
		/// </summary>
        public int PageCount { get; set; }

        /// <summary>
		/// The number items in each page.
		/// </summary>
        public int PageSize { get; set; }

        /// <summary>
		/// 1-based index of the current page being returned.
		/// </summary>
        public int PageNumber { get; set; }

        /// <summary>
		/// Total number of items in the collection.
		/// </summary>
        public int TotalNumberOfItems { get; set; }
    }
}