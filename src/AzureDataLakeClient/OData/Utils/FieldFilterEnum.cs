using System.Collections.Generic;
using AzureDataLakeClient.OData;
using AzureDataLakeClient.OData.Enums;

namespace AzureDataLakeClient.OData.Utils
{
    public class FieldFilterEnum<T> : FieldFilter where T : struct, System.IComparable, System.IConvertible, System.IFormattable 
    {
        private List<T> one_of_value;
        public bool Not;

        public FieldFilterEnum(ExprField field) :
            base(field)
        {
        }

        public void OneOf(params T[] items)
        {
            this.one_of_value = new List<T>(items.Length);
            this.one_of_value.AddRange(items);    
        }

        public override Expr ToExpression()
        {
            if (this.one_of_value != null && this.one_of_value.Count > 0)
            {
                var expr_or = new ExprLogicalOr();
                foreach (var item in this.one_of_value)
                {
                    var t_value = (T) item;
                    string t_string_value = t_value.ToString();
                    var expr_t = new ExprLiteralString(t_string_value);
                    var expr_compare = Expr.GetExprComparison(this.expr_field, expr_t, ComparisonOperation.Equals );
                    expr_or.Add(expr_compare);
                }

                if (this.Not)
                {
                    var expr_not = new ExprLogicalNot(expr_or);
                    return expr_not;
                }
                else
                {
                    return expr_or;
                }
            }
            return null;
        }

        public static string GetEnumDescription(System.Enum value)
        {
            var field_info = value.GetType().GetField(value.ToString());

            var attributes =
              (System.ComponentModel.DescriptionAttribute[])field_info.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}