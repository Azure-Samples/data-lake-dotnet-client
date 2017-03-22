namespace AdlClient.Jobs
{
    public class JobFields
    {
        public OData.ExprField Submitter = new OData.ExprField("submitter");
        public OData.ExprField Name = new OData.ExprField("name");
        public OData.ExprField State = new OData.ExprField("state");
        public OData.ExprField Result = new OData.ExprField("result");
        public OData.ExprField SubmitTime = new OData.ExprField("submitTime");
        public OData.ExprField StartTime = new OData.ExprField("startTime");
        public OData.ExprField EndTime = new OData.ExprField("endTime");
        public OData.ExprField DegreeOfParallelism = new OData.ExprField("degreeOfParallelism");
        public OData.ExprField Priority = new OData.ExprField("priority");
    }
}