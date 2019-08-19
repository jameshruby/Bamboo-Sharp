using System;
using RestSharp;

namespace Bamboo.Sharp.Api.Authentication
{
    internal static class Authenticator
    {
        private static string _userName;
        private static System.Security.SecureString _password;

        internal static RestClient Setup(string userName, System.Security.SecureString password)
        {
            _userName = userName;
            _password = password;

            return Authenticate();
        }

        private static string SS2S(System.Security.SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
            }
            catch (Exception ex){
                throw new Exception("Something wen't horribly wrong when retrieving credentials for authentification" + ex.Message);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        internal static RestClient Authenticate()
        {
            if (string.IsNullOrEmpty(_userName))
                throw new ArgumentNullException("userName");
            if (_password.Length <= 0)
                throw new ArgumentNullException("password");
            
            return new RestClient
            {
              Authenticator = new HttpBasicAuthenticator(_userName, SS2S(_password)),
            };
        }
    }
}
