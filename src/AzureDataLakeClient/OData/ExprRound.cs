namespace AzureDataLakeClient.OData
{
    public class ExprRound : ExprFunction
    {
        public ExprRound(Expr expr) :
            base("round", expr)
        {
        }
    }
}