using System.Collections.Generic;
using Microsoft.Rest.Azure;

namespace AdlClient.Rest
{
    public class PagedIterator<T>
    {
        public FuncGetFirstPage<T> GetFirstPage;
        public FuncGetNextPage<T> GetNextPage;

        public delegate IPage<T> FuncGetFirstPage<T>();
        public delegate IPage<T> FuncGetNextPage<T>(IPage<T> p);

        public IEnumerable<T> EnumerateItems( int top)
        {
            int item_count = 0;
            foreach (var p in this._EnumerateItems())
            {
                yield return p;

                item_count++;

                if ((top > 0) && (item_count >= top))
                {
                    break;
                }

            }
        }

        private IEnumerable<T> _EnumerateItems()
        {
            foreach (var page in this._EnumeratePages())
            {
                foreach (var item in page)
                {
                    yield return item;
                }
            }
        }

        private IEnumerable<IPage<T>> _EnumeratePages()
        {
            var page = this.GetFirstPage();
            yield return page;
            while (!string.IsNullOrEmpty(page.NextPageLink))
            {
                page = this.GetNextPage(page);
                yield return page;
            }
        }

    }
}