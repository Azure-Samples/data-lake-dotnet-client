namespace AdlClient.OData
{
    public class ExprLogicalNot : Expr
    {
        public Expr Expression;

        public ExprLogicalNot(Expr expr)
        {
            this.Expression = expr;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Append("(not");
            writer.Append(this.Expression);
            writer.Append(")");
        }
    }
}