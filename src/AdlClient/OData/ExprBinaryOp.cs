namespace AdlClient.OData
{
    public abstract class ExprBinaryOp : Expr
    {
        public Expr LeftValue;
        public Expr RightValue;
        private string op;

        public ExprBinaryOp(Expr left, Expr right, string op)
        {
            this.LeftValue = left;
            this.RightValue = right;
            this.op = op;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Append("(");
            writer.Append(this.LeftValue);
            writer.Append(" ");
            writer.Append(op);
            writer.Append(" ");
            writer.Append(this.RightValue);
            writer.Append(")");
        }
    }
}