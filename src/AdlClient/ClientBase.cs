namespace AdlClient
{
    public class ClientBase
    {
        public InteractiveAuthentication Authentication;

        internal ClientBase(InteractiveAuthentication auth)
        {
            this.Authentication = auth;
        }
    }
}