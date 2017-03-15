namespace AdlClient.OData
{
    public class ExprNull : Expr
    {
        public ExprNull()
        {
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Append("null");
        }
    }
}