using AzureDataLakeClient.OData.Enums;

namespace AzureDataLakeClient.OData.Utils
{

    public class FieldFilterDateTime : FieldFilter
    {
        private RangeDateTime range;
        public bool Inclusive;
        private FilterCategory category;
        public string Name = "U";

        public FieldFilterDateTime(ExprField field) :
            base(field)
        {
            this.Inclusive = true;
            this.category = FilterCategory.Empty;
        }

        public void InRange(RangeDateTime range)
        {
            this.category = FilterCategory.Range;
            this.range = range;
        }

        public void IsNull()
        {
            this.category = FilterCategory.IsNull;
        }

        public void IsNotNull()
        {
            this.category = FilterCategory.IsNotNull;
        }

        public override Expr ToExpression()
        {
            if (this.category == FilterCategory.Empty)
            {
                return null;
            }
            else if (this.category == FilterCategory.Range)
            {
                // The range must have at least one bound
                if (!this.range.IsBounded)
                {
                    return null;
                }

                var expr_and = new ExprLogicalAnd();

                if (this.range.UpperBound.HasValue)
                {
                    var op = this.Inclusive ? ComparisonOperation.GreaterThanOrEquals : ComparisonOperation.LesserThan;

                    var expr_date = new ExprLiteralDateTime(this.range.UpperBound.Value);
                    var expr_compare = Expr.GetExprComparison(this.expr_field, expr_date, op);
                    expr_and.Add(expr_compare);
                }

                if (this.range.LowerBound.HasValue)
                {
                    var op = this.Inclusive ? ComparisonOperation.GreaterThanOrEquals : ComparisonOperation.GreaterThan;
                    var expr_date = new ExprLiteralDateTime(this.range.LowerBound.Value);
                    var expr_compare = Expr.GetExprComparison(this.expr_field, expr_date, op);
                    expr_and.Add(expr_compare);
                }

                return expr_and;
            }
            else if (this.category ==  FilterCategory.IsNull)
            {
                var op = ComparisonOperation.Equals;
                var expr_null = new ExprNull();
                var expr_compare = Expr.GetExprComparison(this.expr_field, expr_null, op);
                return expr_compare;                
            }
            else if (this.category == FilterCategory.IsNotNull)
            {
                var op = ComparisonOperation.NotEquals;
                var expr_null = new ExprNull();
                var expr_compare = Expr.GetExprComparison(this.expr_field, expr_null, op);
                return expr_compare;
            }
            else
            {
                throw new System.ArgumentException("Unhandled filter category");
            }
        }
    }
}