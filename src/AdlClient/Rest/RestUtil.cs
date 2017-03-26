using System.Collections.Generic;
using Microsoft.Rest.Azure;

namespace AdlClient.Rest
{
    public class RestUtil
    {
        // This method is needed because the Azure .NET SDK does not yet support automatic enumeration of the next pages
        public static IEnumerable<T>EnumItemsInPages<T>(IPage<T> page, System.Func<IPage<T>, IPage<T>> f_get_next_page)
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