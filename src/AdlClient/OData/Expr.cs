using AdlClient.OData.Models;

namespace AdlClient.OData
{
    public abstract class Expr
    {
        public abstract void Write(ExpressionWriter writer);

        public string ToExpressionString()
        {
            var writer = new ExpressionWriter();
            writer.Append(this);
            string s = writer.ToString();
            return s;
        }

        public static ExprBinaryOp GetExprComparison(Expr left, Expr right, ComparisonOperation op)
        {
            if (op == ComparisonOperation.Equals)
            {
                return new OData.ExprEquals(left,right);
            }
            else if (op == ComparisonOperation.NotEquals)
            {
                return new OData.ExprNotEquals(left, right);
            }
            else if (op == ComparisonOperation.GreaterThan)
            {
                return new OData.ExprGreaterThan(left, right);
            }
            else if (op == ComparisonOperation.GreaterThanOrEquals)
            {
                return new OData.ExprGreaterThanOrEquals(left, right);
            }
            else if (op == ComparisonOperation.LesserThan)
            {
                return new OData.ExprLesserThan(left, right);
            }
            else if (op == ComparisonOperation.LesserThanOrEquals)
            {
                return new OData.ExprLesserThanOrEquals(left, right);
            }

            throw new System.ArgumentOutOfRangeException();
        }
    }
}