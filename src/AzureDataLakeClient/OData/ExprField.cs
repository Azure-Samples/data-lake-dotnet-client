namespace AzureDataLakeClient.OData
{
    public class ExprField : Expr
    {
        public string Name;

        public ExprField(string name)
        {
            this.Name = name;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Append(this.Name);
        }
    }
}