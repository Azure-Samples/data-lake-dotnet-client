using System.Collections.Generic;
using AdlClient.OData.Models;

namespace AdlClient.OData.Models
{
    public class FieldFilterEnum<T> : FieldFilter where T : struct, System.IComparable, System.IConvertible, System.IFormattable 
    {
        private List<T> one_of_value;
        public bool Not;
        private EnumFilterCategory Category;

        public FieldFilterEnum(ExprField field) :
            base(field)
        {
            this.Category = EnumFilterCategory.NoFilter;
        }

        public void IsOneOf(params T[] items)
        {
            this.one_of_value = new List<T>(items.Length);
            this.one_of_value.AddRange(items);
            this.Category = EnumFilterCategory.IsOneOf;
        }

        public override Expr ToExpression()
        {
            if (this.Category == EnumFilterCategory.NoFilter)
            {
                return null;
            }
            else if (this.Category == EnumFilterCategory.IsNull)
            {
                return this.CreateIsNullExpr();
            }
            else if (this.Category == EnumFilterCategory.IsNotNull)
            {
                return this.CreateIsNotNullExpr();
            }
            else if (this.Category == EnumFilterCategory.IsOneOf)
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
            else
            {
                string msg = string.Format("Unhandled datetime enum category: \"{0}\"", this.Category);
                throw new System.ArgumentException(msg);

            }
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