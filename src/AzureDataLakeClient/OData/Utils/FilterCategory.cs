namespace AzureDataLakeClient.OData.Utils
{
    public enum DateTimeFilterCategory
    {
        Empty,
        IsNull,
        IsNotNull,
        Range
    }

    public enum StringFilterCategory
    {
        Empty,
        IsNull,
        IsNotNull,
        IsOneOf,
        BeginsWith,
        EndWith,
        Contains
    }
}
 