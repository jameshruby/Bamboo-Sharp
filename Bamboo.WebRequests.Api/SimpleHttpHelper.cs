using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bamboo.WebRequests.Api
{
    public class SimpleHttpHelper : IDisposable
    {
        private string atl_token;
        private bool verbose = false;
        private HttpClient httpClient;

        public bool Verbose { get { return verbose; } set { verbose = value; } }

        public SimpleHttpHelper(string baseUrl, string login, string password, string atl_token)
        {
            this.atl_token = atl_token;

            var cookies = new CookieContainer();
            cookies.Add(new Uri(baseUrl), new Cookie("atl.xsrf.token", atl_token));
            httpClient = new HttpClient(new HttpClientHandler() { CookieContainer = cookies });

            byte[] byteLogin = UTF8Encoding.UTF8.GetBytes(login + ":" + password);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteLogin));
        }

        //var postRequest = new HttpRequestMessage(HttpMethod.Post, request);
        public async Task<bool> ExecutePostRequest(string request, string postParams)
        {
            var response = await httpClient.PostAsync(request, new StringContent(postParams, Encoding.UTF8, "application/x-www-form-urlencoded"));
            var responseMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var responseCode = response.IsSuccessStatusCode;

            if (verbose)
            {
                Console.WriteLine(responseMessage);
            }

            response.Dispose();

            return responseCode && !responseMessage.Contains("Error");

            //dynamic responseObj = JsonConvert.DeserializeObject(responseMessage);
            //var messageFromServer = responseObj.status.Value;
            //var t = responseObj.fieldErrors.variableKey;

            //return response.IsSuccessStatusCode && (!responseMessage.Contains("Error") && responseMessage.Contains("OK"));
        }

        public async Task<string> ExecuteGetRequest(string request)
        {
            //var cookies = new CookieContainer();
            //HttpClient httpClient = new HttpClient(new HttpClientHandler() { CookieContainer = cookies });

            //byte[] login = UTF8Encoding.UTF8.GetBytes(this.login + ":" + password);
            //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(login));

            //cookies.Add(new Uri(request), new Cookie("atl.xsrf.token", atl_token));

            var content = await httpClient.GetStringAsync(request);
            return content;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    httpClient.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SimpleHttpHelper() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

}
