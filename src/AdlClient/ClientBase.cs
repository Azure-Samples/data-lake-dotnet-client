namespace AdlClient
{
    public class ClientBase
    {
        public Authentication Authentication;

        public ClientBase(Authentication auth)
        {
            this.Authentication = auth;
        }
    }
}