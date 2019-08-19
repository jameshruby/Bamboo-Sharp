using System.Threading.Tasks;
using Bamboo.Sharp.Api.Model;
using RestSharp;

namespace Bamboo.Sharp.Api.Services
{
    public class CurrentUserService : BaseService
    {
        //Supported resources
        private const string CurrentUserResource = "currentUser";

        //Base requests
        private readonly IRestRequest _baseCurrentuserRequest = new RestRequest
        {
            Resource = CurrentUserResource,
            Method = Method.GET
        };

        public AUTHENTICATION AuthenticateWithAuthStatus(string userName, System.Security.SecureString password)
        {
            Authentication.Authenticator.Setup(userName, password);
               IRestRequest ff = new RestRequest
               {
                 Resource = "",
                   Method = Method.GET
               };
          
            return Client.ExecuteWithAuthStatus(ff);
        }

        //Implemenations
        public User GetUser()
        {
            return Client.Execute<User>(_baseCurrentuserRequest);
        }

        public string AuthenticateAndGetCurrentUser(string userName, System.Security.SecureString password)
        {
            Authentication.Authenticator.Setup(userName, password);
            User u = Client.Execute<User>(_baseCurrentuserRequest);
            return u != null ? u.FullName : string.Empty;
        }

        public AUTHENTICATION TryAuthenticateAndGetCurrentUser(string userName, System.Security.SecureString password, out string fullUserName)
        {
            Authentication.Authenticator.Setup(userName, password);
            User u;
            var status = Client.TryExecute<User>(_baseCurrentuserRequest, out u);
            fullUserName = u != null ? u.FullName : string.Empty;
            return status;    
        }

        public async Task<User> GetUserAsync()
        {
            return await Client.ExecuteAsync<User>(_baseCurrentuserRequest);
        }
    }
}
