namespace AzureDataLakeClient.OData
{
    public class ExprRaw : Expr
    {
        public string Item;
        public ExprRaw(string s)
        {
            this.Item = s;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Append(this.Item);
        }
    }
}