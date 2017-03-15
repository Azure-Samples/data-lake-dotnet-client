namespace AdlClient.OData
{
    public class ExprSubstring : ExprFunction
    {
        public ExprSubstring(Expr expr1, Expr expr2) :
            base("substring",expr1,expr2)
        {
        }
    }
}