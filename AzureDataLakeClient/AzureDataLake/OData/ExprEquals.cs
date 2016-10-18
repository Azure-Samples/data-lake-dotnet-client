namespace AzureDataLakeClient.OData
{
    public class ExprEquals : ExprBinaryOp
    {
        public ExprEquals(Expr left, Expr right) :
            base(left, right, "eq")
        {
        }
    }
}