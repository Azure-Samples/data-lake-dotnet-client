using System.Collections.Generic;
using System.Linq;
using Microsoft.Rest.Azure;

namespace AdlClient.Rest
{
    public class PagedIterator<T>
    {
        public FuncGetFirstPage GetFirstPage;
        public FuncGetNextPage GetNextPage;

        public delegate IPage<T> FuncGetFirstPage();
        public delegate IPage<T> FuncGetNextPage(IPage<T> p);

        public IEnumerable<T> EnumerateItems(int top)
        {
            var items = this._EnumerateItems().Take(top);
            return items;
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