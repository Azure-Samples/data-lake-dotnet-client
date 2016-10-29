using System.Collections.Generic;
using AzureDataLakeClient.OData.Enums;

namespace AzureDataLakeClient.OData.Utils
{
    public class FieldFilterString : FieldFilter
    {
        private List<string> one_of_text;
        private string begins_with_text;
        private string ends_with_text;
        private string contains_text;
        public bool IgnoreCase;

        public FieldFilterString(ExprField field) :
            base(field)
        {
        }

        public void OneOf(params string[] items)
        {
            this.one_of_text = new List<string>(items.Length);
            this.one_of_text.AddRange(items);    
        }

        public void BeginsWith(string text)
        {
            this.begins_with_text = text;
        }

        public void EndsWith(string text)
        {
            this.ends_with_text = text;
        }

        public void Contains(string text)
        {
            this.contains_text = text;
        }

        public override Expr ToExpression()
        {
            if (this.one_of_text != null && this.one_of_text.Count > 0)
            {
                var expr_or = new ExprLogicalOr();
                foreach (var item in this.one_of_text)
                {
                    var expr1 = FieldFilterString.AlterCase(this.expr_field, this.IgnoreCase);
                    var expr2 = FieldFilterString.AlterCase(new ExprLiteralString(item), this.IgnoreCase);
                    var expr_compare = Expr.GetExprComparison(expr1, expr2, ComparisonOperation.Equals );
                    expr_or.Add(expr_compare);
                }
                return expr_or;
            }
            if (this.contains_text !=null)
            {
                var expr_1 = FieldFilterString.AlterCase(new ExprLiteralString(this.contains_text), this.IgnoreCase);
                var expr_2 = FieldFilterString.AlterCase(this.expr_field, this.IgnoreCase);
                var expr_substringof = new ExprSubstringOf(expr_1,expr_2);
                return expr_substringof;
            }

            if (this.ends_with_text != null)
            {
                var expr_1 = FieldFilterString.AlterCase(this.expr_field, this.IgnoreCase);
                var expr_2 = FieldFilterString.AlterCase(new ExprLiteralString(this.ends_with_text), this.IgnoreCase);
                var expr_endswith= new ExprEndsWith(expr_1, expr_2);
                return expr_endswith;
            }

            if (this.begins_with_text != null)
            {
                var expr_1 = FieldFilterString.AlterCase(this.expr_field, this.IgnoreCase);
                var expr_2 = FieldFilterString.AlterCase(new ExprLiteralString(this.begins_with_text), this.IgnoreCase);
                var expr_startswith = new ExprStartsWith(expr_1, expr_2);
                return expr_startswith;
            }

            return null;
        }


        public static Expr AlterCase(Expr input, bool ignore_case)
        {
            if (ignore_case)
            {
                var new_e = new ExprToLower(input);
                return new_e;
            }
            else
            {
                return input;
            }
        }
    }
}