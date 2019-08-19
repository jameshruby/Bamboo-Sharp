using Bamboo.Sharp.Api.Authentication;
using Bamboo.Sharp.Api.Deserializers;
using Bamboo.Sharp.Api.Exceptions;
using Bamboo.Sharp.Api.Model;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Bamboo.Sharp.Api.Clients
{
    internal class RequestClient
    {
        private static RequestClient _instance;
        private static readonly object Lock = new object();

        private RequestClient()
        {
        }

        public bool Verbose { get { return false; } }

        internal static RequestClient Instance
        {
            get
            {
                lock (Lock)
                {
                    return _instance ?? (_instance = new RequestClient());
                }
            }
        }

        private RestClient _client;

        private void AddCredentialsQueryToRequest(IRestRequest request)
        {
            const string CHAR_PARAM_MORE = "&";
            const string CHAR_PARAM_START = "?";

            string authQuery = "os_authType=basic";

            string beginingDelim = request.Resource.Contains(CHAR_PARAM_START) ? CHAR_PARAM_MORE : CHAR_PARAM_START;

            string resWithAuth = request.Resource.Insert(request.Resource.Length, beginingDelim + authQuery);
            request.Resource = resWithAuth;
        }
        internal void Execute(IRestRequest request)
        {
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/x-www-form-urlencoded"; };

            _client = Authenticator.Authenticate();
            _client.BaseUrl = BambooApi.BaseUrl;

            AddCredentialsQueryToRequest(request);
            var response = _client.Execute(request);

            if (Verbose)
            {
                Console.WriteLine(response.Content);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //throw new UnauthorizedAccessException("Unable to authenticate. Please check your credentials.");
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                //if (!((request.Method == Method.DELETE || request.Method == Method.POST) && response.StatusCode == HttpStatusCode.NoContent))
                //throw new InternalServerError(ExceptionDeserializer.Deserialize(response.Content));
            }
        }


        internal AUTHENTICATION ExecuteWithAuthStatus(IRestRequest request)
        {
            _client = Authenticator.Authenticate();
            _client.BaseUrl = BambooApi.BaseUrl;

            //_client.AddDefaultHeader("Cache-Control", "no-cache");
            //_client.AddDefaultHeader("Pragma", "no-cache");
            //_client.AddDefaultParameter("If-Modified-Since", DateTime.Now, ParameterType.HttpHeader);

            AddCredentialsQueryToRequest(request);
            var response = _client.Execute(request);

            if (Verbose)
            {
                Console.WriteLine(response.Content);
            }
 
            AUTHENTICATION result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = AUTHENTICATION.SUCESSFUL;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                string LoginReason = (string)response.Headers[3].Value;//As for why this info is in the header - https://jira.atlassian.com/browse/CRUC-3595
                if (LoginReason == "AUTHENTICATION_DENIED")
                {
                    result = AUTHENTICATION.DENIED;
                }
                else
                {
                    result = AUTHENTICATION.FAILED;
                }
                //throw new UnauthorizedAccessException("Unable to authenticate. Please check your credentials.");
            }
            else //if (response.StatusCode != HttpStatusCode.OK)
            {
                //lets be defensive for now
                //if (!((request.Method == Method.DELETE || request.Method == Method.POST) && response.StatusCode == HttpStatusCode.NoContent))
                throw new InternalServerError(ExceptionDeserializer.Deserialize(response.Content));
            }

            return result;
        }


        /// <summary>
        /// Use for first login, similar semtantic to TryParse
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        internal AUTHENTICATION TryExecute<T>(IRestRequest request, out T responseData) where T : new()
        {
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/x-www-form-urlencoded"; };

            _client = Authenticator.Authenticate();
            _client.BaseUrl = BambooApi.BaseUrl;

            _client.AddDefaultHeader("Cache-Control", "no-cache");
            _client.AddDefaultHeader("Pragma", "no-cache");
            _client.AddDefaultParameter("If-Modified-Since", DateTime.Now, ParameterType.HttpHeader);


            AddCredentialsQueryToRequest(request);
            var response = _client.Execute<T>(request);

            if (Verbose)
            {
                Console.WriteLine(response.Content);
            }

            AUTHENTICATION result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = AUTHENTICATION.SUCESSFUL;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                string LoginReason = (string)response.Headers[3].Value;//As for why this info is in the header - https://jira.atlassian.com/browse/CRUC-3595
                if (LoginReason == "AUTHENTICATION_DENIED")
                {
                    result = AUTHENTICATION.DENIED;
                }
                else
                {
                    result = AUTHENTICATION.FAILED;
                }
                //throw new UnauthorizedAccessException("Unable to authenticate. Please check your credentials.");
            }
            else //if (response.StatusCode != HttpStatusCode.OK)
            {
                //lets be defensive for now
                //if (!((request.Method == Method.DELETE || request.Method == Method.POST) && response.StatusCode == HttpStatusCode.NoContent))
                throw new InternalServerError(ExceptionDeserializer.Deserialize(response.Content));
            }

            responseData = response.Data;
            return result;
        }


        internal T Execute<T>(IRestRequest request) where T : new()
        {
            _client = Authenticator.Authenticate();
            _client.BaseUrl = BambooApi.BaseUrl;

            AddCredentialsQueryToRequest(request);
            var response = _client.Execute<T>(request);

            if (Verbose)
            {
                Console.WriteLine(response.Content + Environment.NewLine);
            }

            //if (response.StatusCode == HttpStatusCode.Unauthorized)
            //{
            //   string LoginReason = (string)response.Headers[3].Value;//As for why this info is in the header - https://jira.atlassian.com/browse/CRUC-3595
            //    if (LoginReason == "AUTHENTICATION_DENIED") 
            //    {
            //        //TODO return some meaningufull values and get capptchga screen
            //    } 
            //    //throw new UnauthorizedAccessException("Unable to authenticat       e. Please check your credentials.");
            //}

            if (response.StatusCode != HttpStatusCode.OK)
            {
                //for DELETE/POST bamboo sometimes process the request, but not return a response, which is concidered as OK
                //if (!((request.Method == Method.DELETE || request.Method == Method.POST) && response.StatusCode == HttpStatusCode.NoContent))
                //    throw new InternalServerError(ExceptionDeserializer.Deserialize(response.Content));
            }
            return response.Data;
        }

        internal async Task<T> ExecuteAsync<T>(IRestRequest request)
            where T : new()
        {
            var tcs = new TaskCompletionSource<T>();

            _client = Authenticator.Authenticate();
            _client.BaseUrl = BambooApi.BaseUrl;
            _client.ExecuteAsync<T>(request, response => tcs.SetResult(response.Data));

            var task = tcs.Task;

            if (task.Exception != null)
            {
                var message = task.Exception.Flatten().Message;

                throw new InternalServerError(message);
            }

            return await task;
        }
    }
}
