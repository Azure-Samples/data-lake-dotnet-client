namespace AzureDataLakeClient.Analytics
{
    public class JobListSorting
    {
        public JobOrderByField OrderByField;
        public JobOrderByDirection OrderByDirection;


        private static string get_order_field_name(JobOrderByField field)
        {
            if (field == JobOrderByField.None)
            {
                throw new System.ArgumentException();
            }

            string field_name_str = field.ToString();
            return StringUtil.ToLowercaseFirstLetter(field_name_str);
        }

        public string CreateOrderByString()
        {
            if (this.OrderByField != JobOrderByField.None)
            {
                var fieldname = get_order_field_name(this.OrderByField);
                var dir = (this.OrderByDirection == JobOrderByDirection.Ascending) ? "asc" : "desc";

                string orderBy = string.Format("{0} {1}", fieldname, dir);
                return orderBy;
            }

            return null;
        }
    }

    public class GetJobsOptions
    {
        public int Top=0; // 300 is the ADLA limit
        public JobListFilter Filter;
        public JobListSorting Sorting;

        public GetJobsOptions()
        {
            this.Filter = new JobListFilter();
            this.Sorting = new JobListSorting();
        }
    }
}