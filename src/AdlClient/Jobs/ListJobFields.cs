namespace AdlClient.Jobs
{
    public class ListJobFields
    {
        public OData.ExprField field_submitter = new OData.ExprField("submitter");
        public OData.ExprField field_name = new OData.ExprField("name");
        public OData.ExprField field_state = new OData.ExprField("state");
        public OData.ExprField field_result = new OData.ExprField("result");
        public OData.ExprField field_submittime = new OData.ExprField("submitTime");
        public OData.ExprField field_starttime = new OData.ExprField("startTime");
        public OData.ExprField field_endtime = new OData.ExprField("endTime");
        public OData.ExprField field_degreeofparallelism = new OData.ExprField("degreeOfParallelism");
        public OData.ExprField field_priority = new OData.ExprField("priority");
    }
}