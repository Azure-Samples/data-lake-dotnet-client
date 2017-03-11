namespace AzureDataLakeClient.OData
{
    public class ExprLiteralDateTime : Expr
    {
        public System.DateTimeOffset DateTime;

        public ExprLiteralDateTime(System.DateTimeOffset dateTime)
        {
            this.DateTime = dateTime;
        }

        public override void Write(ExpressionWriter writer)
        {
            // due to issue: https://github.com/Azure/autorest/issues/975,
            // date time offsets must be explicitly escaped before being passed to the filter

            string datestring = this.DateTime.ToString("O");
            var escaped_datestring = System.Uri.EscapeDataString(datestring);
            writer.Append(string.Format("datetimeoffset'{0}'", escaped_datestring));
        }
    }
}