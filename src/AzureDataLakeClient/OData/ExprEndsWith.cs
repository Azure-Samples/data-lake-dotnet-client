namespace AzureDataLakeClient.OData
{
    public class ExprEndsWith : ExprFunction
    {
        public ExprEndsWith(Expr expr1, Expr expr2) :
            base("endswith",expr1,expr2)
        {
        }
    }
}