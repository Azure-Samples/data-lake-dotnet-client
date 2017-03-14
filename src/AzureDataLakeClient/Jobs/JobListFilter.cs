using AzureDataLakeClient.OData;
using MSADLA=Microsoft.Azure.Management.DataLake.Analytics;

namespace AzureDataLakeClient.Jobs
{
    public class JobListFilter
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

        public JobListFilter()
        {
            var fields = new JobListFields();

            this.Submitter = new OData.Utils.FieldFilterString(fields.field_submitter);
            this.Name = new OData.Utils.FieldFilterString(fields.field_name);
            this.SubmitTime = new OData.Utils.FieldFilterDateTime(fields.field_submittime);
            this.StartTime = new OData.Utils.FieldFilterDateTime(fields.field_starttime);
            this.EndTime = new OData.Utils.FieldFilterDateTime(fields.field_endtime);
            this.DegreeOfParallelism = new OData.Utils.FieldFilterInteger(fields.field_degreeofparallelism);
            this.Priority = new OData.Utils.FieldFilterInteger(fields.field_priority);
            this.State = new OData.Utils.FieldFilterEnum<MSADLA.Models.JobState>(fields.field_state);
            this.Result = new OData.Utils.FieldFilterEnum<MSADLA.Models.JobResult>(fields.field_result);
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