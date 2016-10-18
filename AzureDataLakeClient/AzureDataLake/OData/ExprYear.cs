namespace AzureDataLakeClient.OData
{
    public class ExprYear : ExprFunction
    {
        public ExprYear(Expr expr) :
            base("year", expr)
        {
        }
    }
}