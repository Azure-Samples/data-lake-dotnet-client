using System.Collections.Generic;
using AzureDataLakeClient.OData.Enums;

namespace AzureDataLakeClient.OData.Utils
{
    public class PropertyFilterInteger : PropertyFilter
    {
        private RangeInteger range;
        private List<int> one_of_list;

        public PropertyFilterInteger(string field_name) :
            base(field_name)
        {
            
        }

        public void InRange(int lower, int upper)
        {
            var r = new RangeInteger(lower,upper);
            this.InRange(r);
        }

        public void InRange(RangeInteger range)
        {
            this.range = range;
            this.one_of_list = null;
        }

        public void OneOf(params int[] values)
        {
            this.range = null;
            this.one_of_list = new List<int>();
            this.one_of_list.AddRange(values);
        }

        public override Expr ToExpression()
        {
            if (this.range == null && this.one_of_list == null)
            {
                return null;
            }

            var expr_and = new ExprLogicalAnd();

            if (this.range != null)
            {
                if (this.range.upper.HasValue)
                {
                    var op = ComparisonOperation.LesserThanOrEquals;
                    var expr_compare = Expr.GetExprComparison(this.expr_field, new ExprLiteralInt(this.range.upper.Value), op);
                    expr_and.Add(expr_compare);
                }

                if (this.range.lower.HasValue)
                {
                    var op = ComparisonOperation.GreaterThanOrEquals ;
                    var expr_compare = Expr.GetExprComparison(this.expr_field, new ExprLiteralInt(this.range.upper.Value), op);
                    expr_and.Add(expr_compare);
                }

            }
            else if (this.one_of_list!=null)
            {
                var expr_or = new ExprLogicalOr();
                foreach (var item in this.one_of_list)
                {
                    var expr2 = new ExprLiteralInt(item);
                    var expr_compare = Expr.GetExprComparison(this.expr_field, expr2, ComparisonOperation.Equals);
                    expr_or.Add(expr_compare);
                }
                return expr_or;
            }
            else
            {
                throw new System.ArgumentOutOfRangeException();
            }

            return expr_and;
        }
    }
}