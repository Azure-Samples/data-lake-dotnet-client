namespace AzureDataLakeClient
{
    public class ClientBase
    {
        public AzureDataLakeClient.Authentication.AuthenticatedSession AuthenticatedSession;

        public ClientBase(AzureDataLakeClient.Authentication.AuthenticatedSession auth_session)
        {
            this.AuthenticatedSession = auth_session;
        }
    }
}