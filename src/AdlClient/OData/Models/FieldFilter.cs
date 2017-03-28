
namespace AdlClient.OData.Models
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
            var op = AdlClient.OData.Models.ComparisonOperation.NotEquals;
            var expr_null = new ExprNull();
            var expr_compare = Expr.GetExprComparison(this.expr_field, expr_null, op);
            return expr_compare;
        }

        protected Expr CreateIsNullExpr()
        {
            var op = AdlClient.OData.Models.ComparisonOperation.Equals;
            var expr_null = new ExprNull();
            var expr_compare = Expr.GetExprComparison(this.expr_field, expr_null, op);
            return expr_compare;
        }
    }
}