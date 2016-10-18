namespace AzureDataLakeClient.OData
{
    public class ExprLiteralInt : Expr
    {
        public int Integer;

        public ExprLiteralInt(int integer)
        {
            this.Integer = integer;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Append(this.Integer.ToString());
        }
    }
}