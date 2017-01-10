
namespace AzureDataLakeClient.OData.Utils
{
    public abstract class FieldFilter
    {
        protected ExprField expr_field;

        protected FieldFilter(ExprField field)
        {
            this.expr_field = new ExprField(field.Name);
        }

        public abstract Expr ToExpression();
    }
}