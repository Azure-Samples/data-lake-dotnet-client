namespace AdlClient.OData
{
    public class ExprLesserThan : ExprBinaryOp
    {
        public ExprLesserThan(Expr left, Expr right) :
            base(left, right, "lt")
        {
        }
    }
}