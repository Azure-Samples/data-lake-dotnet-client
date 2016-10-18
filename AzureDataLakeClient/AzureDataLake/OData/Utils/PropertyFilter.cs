
namespace AzureDataLakeClient.OData.Utils
{
    public abstract class PropertyFilter
    {
        protected ExprField expr_field;

        protected PropertyFilter(string field_name)
        {
            this.expr_field = new ExprField(field_name);
        }

        public abstract Expr ToExpression();
    }
}