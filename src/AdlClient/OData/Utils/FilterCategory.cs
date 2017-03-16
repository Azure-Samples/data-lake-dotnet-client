namespace AdlClient.OData.Utils
{
    public enum DateTimeFilterCategory
    {
        NoFilter,
        IsNull,
        IsNotNull,
        InRange
    }

    public enum StringFilterCategory
    {
        NoFilter,
        IsNull,
        IsNotNull,
        IsOneOf,
        BeginsWith,
        EndWith,
        Contains
    }

    public enum IntegerFilterCategory
    {
        NoFilter,
        IsNull,
        IsNotNull,
        IsOneOf,
        InRange
    }

    public enum EnumFilterCategory
    {
        NoFilter,
        IsNull,
        IsNotNull,
        IsOneOf
    }
}
 