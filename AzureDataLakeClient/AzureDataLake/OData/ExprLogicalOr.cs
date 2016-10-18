namespace AzureDataLakeClient.OData
{
    public class ExprLogicalOr : ExprLogicalList
    {
        public ExprLogicalOr(params Expr[] expressions) :
            base(expressions)
        {
        }

        public override void Write(ExpressionWriter writer)
        {
            this.WriteItems(writer, " or ");
        }
    }
}