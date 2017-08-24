using Microsoft.Azure.Graph.RBAC;

namespace AdlClient.Commands
{
    public class GraphCommands
    {
        internal readonly AuthenticationBase Authentication;
        public readonly Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient GraphClient;

        internal GraphCommands(AuthenticationBase auth)
        {
            this.Authentication = auth;

            this.GraphClient = new Microsoft.Azure.Graph.RBAC.GraphRbacManagementClient(auth.GraphCreds);
            this.GraphClient.TenantID = auth.Tenant;
        }

        public Microsoft.Azure.Graph.RBAC.Models.User GetUser(string user)
        {
            var u = this.GraphClient.Users.Get("saveenr@microsoft.com");
            return u;
        }
    }
}