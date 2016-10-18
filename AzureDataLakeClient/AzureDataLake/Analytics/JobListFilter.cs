using System;
using AzureDataLakeClient.Authentication;
using AzureDataLakeClient.OData;
using AzureDataLakeClient.OData.Enums;
using Microsoft.Azure.Management.DataLake.Analytics.Models;

namespace AzureDataLakeClient.Analytics
{
    public class JobListFilter
    {
        public bool SubmitterIsCurrentUser;
        public OData.Utils.PropertyFilterString Name;
        public OData.Utils.PropertyFilterString Submitter;
        public OData.Utils.PropertyFilterDateTime SubmitTime;
        public OData.Utils.PropertyFilterDateTime StartTime;
        public OData.Utils.PropertyFilterDateTime EndTime;
        public OData.Utils.PropertyFilterInteger DegreeOfParallelism;
        public OData.Utils.PropertyFilterInteger Priority;
        public OData.Utils.PropertyFilterEnum<JobState> State;
        public OData.Utils.PropertyFilterEnum<JobResult> Result;

        public JobListFilter()
        {
            this.Submitter = new OData.Utils.PropertyFilterString("submitter");
            this.Name = new OData.Utils.PropertyFilterString("name");
            this.SubmitTime = new OData.Utils.PropertyFilterDateTime("submitTime");
            this.StartTime = new OData.Utils.PropertyFilterDateTime("startTime");
            this.EndTime = new OData.Utils.PropertyFilterDateTime("endTime");
            this.DegreeOfParallelism = new OData.Utils.PropertyFilterInteger("degreeOfParallelism");
            this.Priority = new OData.Utils.PropertyFilterInteger("priority");
            this.State = new OData.Utils.PropertyFilterEnum<JobState>("state");
            this.Result = new OData.Utils.PropertyFilterEnum<JobResult>("result");
        }

        public string ToFilterString(Authentication.AuthenticatedSession auth_session)
        {
            var expr_and = ToExpression(auth_session);

            var writer = new ExpressionWriter();
            writer.Append(expr_and);
            string text = writer.ToString();
            Console.WriteLine("DEBUG: FILTER {0}", text);
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