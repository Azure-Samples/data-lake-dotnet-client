namespace AzureDataLakeClient.OData
{
    public class ExprLength : ExprFunction
    {
        public ExprLength(Expr expr) :
            base("length", expr)
        {
        }
    }
}