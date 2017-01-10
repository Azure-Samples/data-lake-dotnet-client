using System.Collections.Generic;

namespace AzureDataLakeClient.OData
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

    public abstract class ExprFunction : Expr
    {
        public List<Expr> items;
        private string name;

        public ExprFunction(string name, params Expr[] items)
        {
            this.items = new List<Expr>();
            this.items.AddRange(items);

            this.name = name;
        }

        public override void Write(ExpressionWriter writer)
        {
            writer.Append(name);
            writer.Append("(");
            for (int i = 0; i < this.items.Count; i++)
            {
                if (i > 0)
                {
                    writer.Append(", ");
                }
                writer.Append(this.items[i]);
            }
            writer.Append(")");
        }
    }


}