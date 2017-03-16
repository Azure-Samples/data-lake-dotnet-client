using System.Collections.Generic;
using System.Linq;
using AdlClient.OData.Enums;

namespace AdlClient.OData.Utils
{
    public class FieldFilterString : FieldFilter
    {
        private List<string> values;

        public bool IgnoreCase=true;
        private StringFilterCategory Category;

        public FieldFilterString(ExprField field) :
            base(field)
        {
            this.Category = StringFilterCategory.NoFilter;
        }

        public void IsOneOf(params string[] items)
        {
            this.values = items.ToList();
            this.Category = StringFilterCategory.IsOneOf;
        }

        public void BeginsWith(params string[] items)
        {
            this.values = items.ToList();
            this.Category = StringFilterCategory.BeginsWith;
        }

        public void EndsWith(params string[] items)
        {
            this.values = items.ToList();
            this.Category = StringFilterCategory.EndWith;
        }

        public void Contains(params string[] items)
        {
            this.values = items.ToList();
            this.Category = StringFilterCategory.Contains;
        }

        public override Expr ToExpression()
        {
            if (this.Category == StringFilterCategory.IsOneOf && this.values != null && this.values.Count > 0)
            {
                var expr_or = new ExprLogicalOr();
                foreach (var item in this.values)
                {
                    var expr1 = FieldFilterString.AlterCase(this.expr_field, this.IgnoreCase);
                    var expr2 = FieldFilterString.AlterCase(new ExprLiteralString(item), this.IgnoreCase);
                    var expr_compare = Expr.GetExprComparison(expr1, expr2, ComparisonOperation.Equals );
                    expr_or.Add(expr_compare);
                }
                return expr_or;
            }
            else if (this.Category == StringFilterCategory.Contains && this.values != null)
            {
                var expr_or = new ExprLogicalOr();

                foreach (var item in this.values)
                {
                    var expr_1 = FieldFilterString.AlterCase(new ExprLiteralString(item), this.IgnoreCase);
                    var expr_2 = FieldFilterString.AlterCase(this.expr_field, this.IgnoreCase);

                    var expr_substringof = new ExprSubstringOf(expr_1, expr_2);

                    expr_or.Add(expr_substringof);
                }
                return expr_or;
            }
            else if (this.Category == StringFilterCategory.EndWith && this.values != null)
            {

                var expr_or = new ExprLogicalOr();

                foreach (var item in this.values)
                {
                    var expr_1 = FieldFilterString.AlterCase(this.expr_field, this.IgnoreCase);
                    var expr_2 = FieldFilterString.AlterCase(new ExprLiteralString(item), this.IgnoreCase);

                    var expr_endswith = new ExprEndsWith(expr_1, expr_2);

                    expr_or.Add(expr_endswith);
                }
                return expr_or;
            }
            else if (this.Category == StringFilterCategory.BeginsWith && this.values != null)
            {

                var expr_or = new ExprLogicalOr();

                foreach (var item in this.values)
                {
                    var expr_1 = FieldFilterString.AlterCase(this.expr_field, this.IgnoreCase);
                    var expr_2 = FieldFilterString.AlterCase(new ExprLiteralString(item), this.IgnoreCase);

                    var expr_startswith = new ExprStartsWith(expr_1, expr_2);

                    expr_or.Add(expr_startswith);
                }
                return expr_or;
            }
            else if (this.Category == StringFilterCategory.NoFilter)
            {
                return null;
            }
            else if (this.Category == StringFilterCategory.IsNotNull)
            {
                return this.CreateIsNotNullExpr();
            }
            else if (this.Category == StringFilterCategory.IsNull)
            {
                return this.CreateIsNullExpr();
            }
            else
            {
                string msg = string.Format("Unhandled string filter category: \"{0}\"", this.Category);
                throw new System.ArgumentException(msg);
            }
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