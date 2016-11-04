using AzureDataLakeClient.OData.Enums;

namespace AzureDataLakeClient.OData.Utils
{
    public class FieldFilterDateTime : FieldFilter
    {
        private RangeDateTime range;
        public bool Inclusive;

        public FieldFilterDateTime(ExprField field) :
            base(field)
        {
        }

        public void InRange(RangeDateTime range)
        {
            this.range = range;
        }


        public override Expr ToExpression()
        {
            if (this.range == null)
            {
                return null;
            }

            if (!this.range.HasBoundary)
            {
                return null;
            }

            var expr_and = new ExprLogicalAnd();

            if (this.range.upper.HasValue)
            {
                var op = this.Inclusive ? ComparisonOperation.GreaterThanOrEquals : ComparisonOperation.LesserThan;

                var expr_date = new ExprLiteralDateTime(this.range.upper.Value);
                var expr_compare = Expr.GetExprComparison(this.expr_field, expr_date, op);
                expr_and.Add(expr_compare);
            }

            if (this.range.lower.HasValue)
            {
                var op = this.Inclusive ? ComparisonOperation.GreaterThanOrEquals : ComparisonOperation.GreaterThan;
                var expr_date = new ExprLiteralDateTime(this.range.lower.Value);
                var expr_compare = Expr.GetExprComparison(this.expr_field, expr_date, op);
                expr_and.Add(expr_compare);
            }

            return expr_and;
        }
    }
}