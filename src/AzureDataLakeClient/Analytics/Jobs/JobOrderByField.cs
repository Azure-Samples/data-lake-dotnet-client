namespace AzureDataLakeClient.Analytics.Jobs
{
    public enum JobOrderByField
    {
        None,
        SubmitTime,
        Submitter,
        DegreeOfParallelism,
        EndTime,
        Name,
        Priority,
        Result
    }
}