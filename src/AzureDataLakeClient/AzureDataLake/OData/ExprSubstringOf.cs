namespace AzureDataLakeClient.OData
{
    public class ExprSubstringOf : ExprFunction
    {
        public ExprSubstringOf(Expr expr1, Expr expr2) :
            base("substringof",expr1,expr2)
        {
        }
    }

    public class ExprStartsWith : ExprFunction
    {
        public ExprStartsWith(Expr expr1, Expr expr2) :
            base( "startswith", expr1, expr2)
        {
        }
    }

    public class ExprEndsWith : ExprFunction
    {
        public ExprEndsWith(Expr expr1, Expr expr2) :
            base("endswith",expr1,expr2)
        {
        }
    }

}