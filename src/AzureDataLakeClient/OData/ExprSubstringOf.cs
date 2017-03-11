namespace AzureDataLakeClient.OData
{
    public class ExprSubstringOf : ExprFunction
    {
        public ExprSubstringOf(Expr expr1, Expr expr2) :
            base("substringof",expr1,expr2)
        {
        }
    }
}