using AdlClient.Authentication;

namespace AdlClient
{
    public class ClientBase
    {
        public AuthenticatedSession AuthenticatedSession;

        public ClientBase(AuthenticatedSession auth_session)
        {
            this.AuthenticatedSession = auth_session;
        }
    }
}