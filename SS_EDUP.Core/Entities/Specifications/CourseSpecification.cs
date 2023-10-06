using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SS_EDUP.Core.Entities.Specifications
{
    public static class Courses
    {
        // all product specifications
        public class All : Specification<Course>
        {
            public All()
            {
                Query.Include(x => x.Category).Include(x=>x.Author);
            }
        }

        //public class ByPrice : Specification<Course>
        //{
        //    public ByPrice(decimal from, decimal to)
        //    {
        //        Query
        //            .Where(x => x.Price >= from && x.Price <= to)
        //            .Include(x => x.Category)
        //            .Include(x => x.Author);
        //    }
        //}

        public class ByCategory : Specification<Course>
        {
            public ByCategory(int categoryId)
            {
                Query
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .Where(c=>c.CategoryId== categoryId);
            }
        }

        public class ByIds : Specification<Course>
        {
            public ByIds(int[] ids)
            {
                Query
                   .Where(x => ids.Contains(x.Id))
                   .Include(x => x.Category)
                   .Include(x=>x.Author);
            }
        }

        public class ByAuthor : Specification<Course>
        {
            public ByAuthor(string authorId)
            {
                Query
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                   .Where(c => c.AuthorId == authorId);
            }
        }
    }
}
