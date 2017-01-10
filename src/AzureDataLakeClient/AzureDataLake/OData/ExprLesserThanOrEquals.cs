namespace AzureDataLakeClient.OData
{
    public class ExprLesserThanOrEquals : ExprBinaryOp
    {
        public ExprLesserThanOrEquals(Expr left, Expr right) :
            base(left, right, "le")
        {
        }
    }
}