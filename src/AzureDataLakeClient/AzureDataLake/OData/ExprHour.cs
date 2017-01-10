namespace AzureDataLakeClient.OData
{
    public class ExprHour: ExprFunction
    {
        public ExprHour(Expr expr) :
            base("hour", expr)
        {
        }
    }
}