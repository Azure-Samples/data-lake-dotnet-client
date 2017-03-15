namespace AdlClient.OData
{
    public class ExprMinute : ExprFunction
    {
        public ExprMinute(Expr expr) :
            base("minute", expr)
        {
        }
    }
}