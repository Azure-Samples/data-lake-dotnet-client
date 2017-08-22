using System.Collections.Generic;
using Microsoft.Rest.Azure;

namespace AdlClient.Rest
{
    public class PagedIterator<T>
    {
        public RestUtil.GetFirstPage<T> GetFirstPage;
        public RestUtil.GetNextPage<T> GetNextPage;
    }
    public class RestUtil
    {


        public delegate IPage<T> GetFirstPage<T>();
        public delegate IPage<T> GetNextPage<T>(IPage<T> p);

        public static IEnumerable<T> EnumItems<T>(
            PagedIterator<T> pageiter,
            int top)
        {
            int item_count = 0;
            foreach (var p in _Enumerate(pageiter.GetFirstPage, pageiter.GetNextPage))
            {
                yield return p;

                item_count++;

                if ((top > 0) && (item_count >= top))
                {
                    break;
                }

            }
        }

        private static IEnumerable<T> _Enumerate<T>(GetFirstPage<T> f_get_first_page, GetNextPage<T> f_get_next_page)
        {
            var page = f_get_first_page();
            // Handle the first page
            foreach (var item in page)
            {
                yield return item;
            }

            // Handle the remaining pages
            while (!string.IsNullOrEmpty(page.NextPageLink))
            {
                page = f_get_next_page(page);

                foreach (var item in page)
                {
                    yield return item;
                }
            }
        }
    }
}