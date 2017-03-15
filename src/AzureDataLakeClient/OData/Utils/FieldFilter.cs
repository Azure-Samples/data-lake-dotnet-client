
namespace AdlClient.OData.Utils
{
    public abstract class FieldFilter
    {
        protected ExprField expr_field;

        protected FieldFilter(ExprField field)
        {
            this.expr_field = new ExprField(field.Name);
        }

        public abstract Expr ToExpression();


        protected Expr CreateIsNotNullExpr()
        {
            var op = Enums.ComparisonOperation.NotEquals;
            var expr_null = new ExprNull();
            var expr_compare = Expr.GetExprComparison(this.expr_field, expr_null, op);
            return expr_compare;
        }

        protected Expr CreateIsNullExpr()
        {
            var op = Enums.ComparisonOperation.Equals;
            var expr_null = new ExprNull();
            var expr_compare = Expr.GetExprComparison(this.expr_field, expr_null, op);
            return expr_compare;
        }
    }
}