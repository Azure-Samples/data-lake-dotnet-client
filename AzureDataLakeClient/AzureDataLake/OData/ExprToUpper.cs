namespace AzureDataLakeClient.OData
{
    public class ExprToUpper : ExprFunction
    {
        public ExprToUpper(Expr expr) :
            base("toupper",expr)
        {
        }
    }
}