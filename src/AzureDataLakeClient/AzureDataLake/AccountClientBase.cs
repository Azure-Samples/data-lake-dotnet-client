using AzureDataLakeClient.Authentication;

namespace AzureDataLakeClient
{
    public class AccountClientBase : ClientBase
    {
        public string Account;

        public AccountClientBase(string account, AuthenticatedSession auth_session) :
            base(auth_session)
        {
            this.Account = account;
        }
    }
}