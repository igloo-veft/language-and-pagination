using System.Collections.Generic;

namespace CoursesAPI.Models
{
    public class Envelope<T>
    {
        public IEnumerable<T> Items { get; set; }
        public Paging Paging { get; set; }
    }
}