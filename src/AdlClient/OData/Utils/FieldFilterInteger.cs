using System.Collections.Generic;
using AdlClient.OData.Enums;

namespace AdlClient.OData.Utils
{
    public class FieldFilterInteger : FieldFilter
    {
        private RangeInteger range;
        private List<int> one_of_list;
        private IntegerFilterCategory Category;

        public FieldFilterInteger(ExprField field) :
            base(field)
        {
            this.Category = IntegerFilterCategory.NoFilter;    
        }

        public void IsInRange(int lower, int upper)
        {
            var r = new RangeInteger(lower,upper);
            this.IsInRange(r);
            this.Category = IntegerFilterCategory.IsInRange;
        }

        public void IsInRange(RangeInteger range)
        {
            this.range = range;
            this.one_of_list = null;
            this.Category = IntegerFilterCategory.IsInRange;
        }

        public void IsOneOf(params int[] values)
        {
            this.range = null;
            this.one_of_list = new List<int>();
            this.one_of_list.AddRange(values);
            this.Category = IntegerFilterCategory.IsOneOf;
        }

        public override Expr ToExpression()
        {
            if (this.Category == IntegerFilterCategory.NoFilter)
            {
                return null;
            }
            else if (this.Category == IntegerFilterCategory.IsNull)
            {
                return this.CreateIsNullExpr();
            }
            else if (this.Category == IntegerFilterCategory.IsNotNull)
            {
                return this.CreateIsNullExpr();
            }
            else if (this.Category == IntegerFilterCategory.IsInRange)
            {
                var expr_and = new ExprLogicalAnd();
                if (this.range.UpperBound.HasValue)
                {
                    var op = ComparisonOperation.LesserThanOrEquals;
                    var expr_compare = Expr.GetExprComparison(this.expr_field, new ExprLiteralInt(this.range.UpperBound.Value), op);
                    expr_and.Add(expr_compare);
                }

                if (this.range.LowerBound.HasValue)
                {
                    var op = ComparisonOperation.GreaterThanOrEquals;
                    var expr_compare = Expr.GetExprComparison(this.expr_field, new ExprLiteralInt(this.range.UpperBound.Value), op);
                    expr_and.Add(expr_compare);
                }

                return expr_and;
            }
            else if (this.Category == IntegerFilterCategory.IsOneOf)
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
                string msg = string.Format("Unhandled datetime integer category: \"{0}\"", this.Category);
                throw new System.ArgumentException(msg);
            }
        }
    }
}