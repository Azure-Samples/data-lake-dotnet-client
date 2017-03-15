namespace AdlClient.OData
{
    public class ExprDay: ExprFunction
    {
        public ExprDay(Expr expr) :
            base("day", expr)
        {
        }
    }
}