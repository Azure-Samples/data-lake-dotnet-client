using AdlClient.OData;
using MSADLA=Microsoft.Azure.Management.DataLake.Analytics;

namespace AdlClient.Jobs
{
    public class JobListingFilter
    {
        public readonly OData.Utils.FieldFilterString Name;
        public readonly OData.Utils.FieldFilterString Submitter;
        public readonly OData.Utils.FieldFilterDateTime SubmitTime;
        public readonly OData.Utils.FieldFilterDateTime StartTime;
        public readonly OData.Utils.FieldFilterDateTime EndTime;
        public readonly OData.Utils.FieldFilterInteger DegreeOfParallelism;
        public readonly OData.Utils.FieldFilterInteger Priority;
        public readonly OData.Utils.FieldFilterEnum<MSADLA.Models.JobState> State;
        public readonly OData.Utils.FieldFilterEnum<MSADLA.Models.JobResult> Result;

        public JobListingFilter()
        {
            var fields = new JobFields();

            this.Submitter = new OData.Utils.FieldFilterString(fields.Submitter);
            this.Name = new OData.Utils.FieldFilterString(fields.Name);
            this.SubmitTime = new OData.Utils.FieldFilterDateTime(fields.SubmitTime);
            this.StartTime = new OData.Utils.FieldFilterDateTime(fields.StartTime);
            this.EndTime = new OData.Utils.FieldFilterDateTime(fields.EndTime);
            this.DegreeOfParallelism = new OData.Utils.FieldFilterInteger(fields.DegreeOfParallelism);
            this.Priority = new OData.Utils.FieldFilterInteger(fields.Priority);
            this.State = new OData.Utils.FieldFilterEnum<MSADLA.Models.JobState>(fields.State);
            this.Result = new OData.Utils.FieldFilterEnum<MSADLA.Models.JobResult>(fields.Result);
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