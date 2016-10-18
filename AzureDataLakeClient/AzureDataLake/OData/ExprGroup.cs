namespace AzureDataLakeClient.OData
{
    public class ExprGroup : Expr
    {
        public Expr Expression;
        public ExprGroup( Expr expression)
        {
            this.Expression = expression;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Append("(");
            writer.Append(this.Expression);
            writer.Append(")");
        }
    }
}