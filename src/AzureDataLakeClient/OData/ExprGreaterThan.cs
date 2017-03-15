namespace AdlClient.OData
{
    public class ExprGreaterThan : ExprBinaryOp
    {
        public ExprGreaterThan(Expr left, Expr right) :
            base(left, right,"gt")
        {
        }
    }
}