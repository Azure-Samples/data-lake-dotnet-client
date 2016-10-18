namespace AzureDataLakeClient.OData
{
    public class ExprGreaterThanOrEquals : ExprBinaryOp
    {
        public ExprGreaterThanOrEquals(Expr left, Expr right) :
            base(left, right, "ge")
        {
        }
    }
}