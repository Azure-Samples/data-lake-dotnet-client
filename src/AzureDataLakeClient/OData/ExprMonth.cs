namespace AzureDataLakeClient.OData
{
    public class ExprMonth : ExprFunction
    {
        public ExprMonth(Expr expr) :
            base("month", expr)
        {
        }
    }
}