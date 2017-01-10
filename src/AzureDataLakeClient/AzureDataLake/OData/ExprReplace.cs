namespace AzureDataLakeClient.OData
{
    public class ExprReplace : ExprFunction
    {
        public ExprReplace(Expr expr1, Expr expr2, Expr expr3) :
            base("indexof",expr1,expr2,expr3)
        {
        }
    }
}