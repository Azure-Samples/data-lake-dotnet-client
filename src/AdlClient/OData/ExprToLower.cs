namespace AdlClient.OData
{
    public class ExprToLower : ExprFunction
    {
        public ExprToLower(Expr expr) :
            base("tolower", expr)
        {
        }
    }
}