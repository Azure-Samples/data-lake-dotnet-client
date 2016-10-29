using System;
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.OData;
using Microsoft.Azure.Management.DataLake.Analytics.Models;

namespace AzureDataLakeClient.Analytics
{
    public class JobListFilter
    {
        public bool SubmitterIsCurrentUser;
        public OData.Utils.FieldFilterString Name;
        public OData.Utils.FieldFilterString Submitter;
        public OData.Utils.FieldFilterDateTime SubmitTime;
        public OData.Utils.FieldFilterDateTime StartTime;
        public OData.Utils.FieldFilterDateTime EndTime;
        public OData.Utils.FieldFilterInteger DegreeOfParallelism;
        public OData.Utils.FieldFilterInteger Priority;
        public OData.Utils.FieldFilterEnum<JobState> State;
        public OData.Utils.FieldFilterEnum<JobResult> Result;

        public JobListFilter()
        {
            var field_submitter = new OData.ExprField("submitter");
            var field_name = new OData.ExprField("name");
            var field_state = new OData.ExprField("state");
            var field_result = new OData.ExprField("result");
            var field_submittime = new OData.ExprField("submitTime");
            var field_starttime = new OData.ExprField("startTime");
            var field_endtime = new OData.ExprField("endTime");
            var field_degreeofparallelism = new OData.ExprField("degreeOfParallelism");
            var field_priority = new OData.ExprField("priority");

            this.Submitter = new OData.Utils.FieldFilterString(field_submitter);
            this.Name = new OData.Utils.FieldFilterString(field_name);
            this.SubmitTime = new OData.Utils.FieldFilterDateTime(field_submittime);
            this.StartTime = new OData.Utils.FieldFilterDateTime(field_starttime);
            this.EndTime = new OData.Utils.FieldFilterDateTime(field_endtime);
            this.DegreeOfParallelism = new OData.Utils.FieldFilterInteger(field_degreeofparallelism);
            this.Priority = new OData.Utils.FieldFilterInteger(field_priority);
            this.State = new OData.Utils.FieldFilterEnum<JobState>(field_state);
            this.Result = new OData.Utils.FieldFilterEnum<JobResult>(field_result);
        }

        public string ToFilterString(Authentication.AuthenticatedSession auth_session)
        {
            var expr_and = ToExpression(auth_session);

            var writer = new ExpressionWriter();
            writer.Append(expr_and);
            string text = writer.ToString();
            return text;
        }

        private Expr ToExpression(AuthenticatedSession auth_session)
        {
            var expr_and = new ExprLogicalAnd();
            var col_submitter = new OData.ExprField("submitter");

            expr_and.Add(this.DegreeOfParallelism?.ToExpression());
            expr_and.Add(this.Submitter?.ToExpression());
            expr_and.Add(this.Priority?.ToExpression());
            expr_and.Add(this.Name?.ToExpression());
            expr_and.Add(this.SubmitTime?.ToExpression());
            expr_and.Add(this.State?.ToExpression());
            expr_and.Add(this.Result?.ToExpression());

            if (this.SubmitterIsCurrentUser)
            {
                var exprStringLiteral = new OData.ExprLiteralString(auth_session.Token.DisplayableId);

                var expr0 = new OData.ExprToLower(col_submitter);
                var expr1 = new OData.ExprToLower(exprStringLiteral);

                var expr_compare = new ExprEquals(expr0, expr1);
                expr_and.Add(expr_compare);
            }
            return expr_and;
        }
    }
}