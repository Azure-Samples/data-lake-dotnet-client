namespace AdlClient
{
    public class ClientBase
    {
        public AuthenticationBase Authentication;

        internal ClientBase(AuthenticationBase auth)
        {
            this.Authentication = auth;
        }
    }
}