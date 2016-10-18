namespace AzureDataLakeClient.OData
{
    public class ExprIndexOf : ExprFunction
    {
        public ExprIndexOf(Expr expr1, Expr expr2) :
            base("indexof", expr1, expr2)
        {
        }
    }
}