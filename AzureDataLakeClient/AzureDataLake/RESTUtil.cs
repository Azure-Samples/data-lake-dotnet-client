using System.Collections.Generic;
using Microsoft.Rest.Azure;

namespace AzureDataLakeClient
{
    public class RESTUtil
    {
        public static IEnumerable<T> EnumItemsInPages<T>(IPage<T> page, System.Func<IPage<T>, IPage<T>> f_get_next_page)
        {
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