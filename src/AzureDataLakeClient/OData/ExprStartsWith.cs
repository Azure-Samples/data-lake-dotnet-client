namespace AzureDataLakeClient.OData
{
    public class ExprStartsWith : ExprFunction
    {
        public ExprStartsWith(Expr expr1, Expr expr2) :
            base( "startswith", expr1, expr2)
        {
        }
    }
}