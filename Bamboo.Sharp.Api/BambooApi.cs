using Bamboo.Sharp.Api.Authentication;
using Bamboo.Sharp.Api.Factories;
using Bamboo.Sharp.Api.Services;

namespace Bamboo.Sharp.Api
{
    public class BambooApi
    {
        internal static string BaseUrl { get; set; }

        public T GetService<T>() where T : IService, new()
        {
            return ServiceFactory.GetService<T>();
        }

        public BambooApi(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        private void SetupAuthenticatorUnsafe(string userName, string password)
        {
            var secure = new System.Security.SecureString();
            foreach (char c in password)
            {
                secure.AppendChar(c);
            }
            Authenticator.Setup(userName, secure);
        }

        [System.Obsolete("This is unsafe password handling and should be used only for testing/internal purpouses")]
        public Model.User AuthenticateAndGetCurrentUser(string userName, string password)
        {
            SetupAuthenticatorUnsafe(userName, password);
            CurrentUserService userInfoService = new CurrentUserService();
            Model.User user = userInfoService.GetUser();
            return user;
        }

        public Model.User AuthenticateAndGetCurrentUser(string userName, System.Security.SecureString password)
        {
            Authenticator.Setup(userName, password);
            CurrentUserService userInfoService = new CurrentUserService();
            Model.User user = userInfoService.GetUser();
            return user;
        }
        
        public BambooApi(string baseUrl, string userName, string password)
        {
            BaseUrl = baseUrl;
            SetupAuthenticatorUnsafe(userName, password);
        }
        public BambooApi(string baseUrl, string userName, System.Security.SecureString password)
        {
            BaseUrl = baseUrl;
            Authenticator.Setup(userName, password);
        }
    }
}