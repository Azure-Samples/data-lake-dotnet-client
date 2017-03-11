namespace AzureDataLakeClient.OData
{
    public class ExprCeiling : ExprFunction
    {
        public ExprCeiling(Expr expr) :
            base("ceiling",expr)
        {
        }
    }
}