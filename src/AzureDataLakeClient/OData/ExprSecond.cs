namespace AdlClient.OData
{
    public class ExprSecond : ExprFunction
    {
        public ExprSecond(Expr expr) :
            base("second", expr)
        {
        }
    }
}