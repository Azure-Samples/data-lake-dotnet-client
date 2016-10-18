namespace AzureDataLakeClient.OData
{
    public class ExprNotEquals : ExprBinaryOp
    {
        public ExprNotEquals(Expr left, Expr right) :
            base(left, right, "ne")
        {
        }
    }
}