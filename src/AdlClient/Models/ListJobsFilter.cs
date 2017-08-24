using AdlClient.OData;
using MSADLA=Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Models
{
    public class JobListingFilter
    {
        public readonly OData.Models.FieldFilterString Name;
        public readonly OData.Models.FieldFilterString Submitter;
        public readonly OData.Models.FieldFilterDateTime SubmitTime;
        public readonly OData.Models.FieldFilterDateTime StartTime;
        public readonly OData.Models.FieldFilterDateTime EndTime;
        public readonly OData.Models.FieldFilterInteger DegreeOfParallelism;
        public readonly OData.Models.FieldFilterInteger Priority;
        public readonly OData.Models.FieldFilterEnum<MSADLA.Models.JobState> State;
        public readonly OData.Models.FieldFilterEnum<MSADLA.Models.JobResult> Result;

        public JobListingFilter()
        {
            var fields = new JobExprFields();

            this.Submitter = new OData.Models.FieldFilterString(fields.Submitter);
            this.Name = new OData.Models.FieldFilterString(fields.Name);
            this.SubmitTime = new OData.Models.FieldFilterDateTime(fields.SubmitTime);
            this.StartTime = new OData.Models.FieldFilterDateTime(fields.StartTime);
            this.EndTime = new OData.Models.FieldFilterDateTime(fields.EndTime);
            this.DegreeOfParallelism = new OData.Models.FieldFilterInteger(fields.DegreeOfParallelism);
            this.Priority = new OData.Models.FieldFilterInteger(fields.Priority);
            this.State = new OData.Models.FieldFilterEnum<MSADLA.Models.JobState>(fields.State);
            this.Result = new OData.Models.FieldFilterEnum<MSADLA.Models.JobResult>(fields.Result);
        }

        public string ToFilterString()
        {
            var expr_and = ToExpression();

            var writer = new ExpressionWriter();
            writer.Append(expr_and);
            string text = writer.ToString();
            return text;
        }

        private Expr ToExpression()
        {
            var expr_and = new ExprLogicalAnd();

            expr_and.Add(this.DegreeOfParallelism.ToExpression());
            expr_and.Add(this.Submitter.ToExpression());
            expr_and.Add(this.Priority.ToExpression());
            expr_and.Add(this.Name.ToExpression());
            expr_and.Add(this.SubmitTime.ToExpression());
            expr_and.Add(this.StartTime.ToExpression());
            expr_and.Add(this.EndTime.ToExpression());
            expr_and.Add(this.State.ToExpression());
            expr_and.Add(this.Result.ToExpression());

            return expr_and;
        }
    }
}