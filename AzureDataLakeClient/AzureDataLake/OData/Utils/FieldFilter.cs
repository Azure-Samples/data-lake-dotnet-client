
namespace AzureDataLakeClient.OData.Utils
{
    public abstract class FieldFilter
    {
        protected ExprField expr_field;

        protected FieldFilter(string field_name)
        {
            this.expr_field = new ExprField(field_name);
        }

        public abstract Expr ToExpression();
    }
}