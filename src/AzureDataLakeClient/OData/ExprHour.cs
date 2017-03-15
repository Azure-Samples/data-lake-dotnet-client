namespace AdlClient.OData
{
    public class ExprHour: ExprFunction
    {
        public ExprHour(Expr expr) :
            base("hour", expr)
        {
        }
    }
}