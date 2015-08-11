using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bamboo.WebRequests.Api
{
    public class SimpleHttpHelper
    {
        private string password;
        private string login;
        private string atl_token;
        private bool verbose = false;

        public bool Verbose { get { return verbose; } set { verbose = value; } }
        
        public SimpleHttpHelper(string login, string password, string atl_token)
        {
            this.login = login;
            this.password = password;
            this.atl_token = atl_token;
        }

        public async Task<bool> ExecutePostRequest(string request, string postParams)
        {
            var cookies = new CookieContainer();
            HttpClient httpClient = new HttpClient(new HttpClientHandler() { CookieContainer = cookies });
            byte[] login = UTF8Encoding.UTF8.GetBytes(this.login + ":" + password);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(login));

            cookies.Add(new Uri(request), new Cookie("atl.xsrf.token", atl_token));
            var postRequest = new HttpRequestMessage(HttpMethod.Post, request);


            var response = await httpClient.PostAsync(request, new StringContent(postParams, Encoding.UTF8, "application/x-www-form-urlencoded"));
            var responseMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (verbose)
            {
                Console.WriteLine(responseMessage);
            }

            //dynamic responseObj = JsonConvert.DeserializeObject(responseMessage);
            //var messageFromServer = responseObj.status.Value;
            //var t = responseObj.fieldErrors.variableKey;

            return response.IsSuccessStatusCode && !responseMessage.Contains("Error");
        }

        public async Task<string> ExecuteGetRequest(string request)
        {
            var cookies = new CookieContainer();
            HttpClient httpClient = new HttpClient(new HttpClientHandler() { CookieContainer = cookies });

            byte[] login = UTF8Encoding.UTF8.GetBytes(this.login + ":" + password);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(login));

            cookies.Add(new Uri(request), new Cookie("atl.xsrf.token", atl_token));

            var content = await httpClient.GetStringAsync(request);
            return content;
        }

    
    }

}
