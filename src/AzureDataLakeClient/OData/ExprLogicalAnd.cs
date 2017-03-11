using System;

namespace AzureDataLakeClient.OData
{
    public class ExprLogicalAnd : ExprLogicalList
    {

        public ExprLogicalAnd(params Expr[] expressions) :
            base(expressions)
        {
        }

        public override void Write(ExpressionWriter writer)
        {
            this.WriteItems(writer, " and ");
        }
    }
}