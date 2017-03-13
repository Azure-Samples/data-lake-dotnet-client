using AzureDataLakeClient.Authentication;

namespace AzureDataLakeClient
{
    public class AccountClientBase : ClientBase
    {
        public AccountClientBase(AuthenticatedSession auth_session) :
            base(auth_session)
        {
            this.AuthenticatedSession = auth_session;
        }
    }
}