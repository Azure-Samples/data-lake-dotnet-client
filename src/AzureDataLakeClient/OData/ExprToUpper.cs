namespace AdlClient.OData
{
    public class ExprToUpper : ExprFunction
    {
        public ExprToUpper(Expr expr) :
            base("toupper",expr)
        {
        }
    }
}