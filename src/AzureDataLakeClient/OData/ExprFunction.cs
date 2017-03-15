using System.Collections.Generic;

namespace AdlClient.OData
{
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