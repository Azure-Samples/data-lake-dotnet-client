namespace AdlClient
{
    public class ClientBase
    {
        public Authentication Authentication;

        internal ClientBase(Authentication auth)
        {
            this.Authentication = auth;
        }
    }
}