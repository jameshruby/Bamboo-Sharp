using Bamboo.Sharp.Api.Authentication;
using Bamboo.Sharp.Api.Deserializers;
using Bamboo.Sharp.Api.Exceptions;
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

        internal void Execute(IRestRequest request)
        {
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/x-www-form-urlencoded"; };


            _client = Authenticator.Authenticate();
            _client.BaseUrl = BambooApi.BaseUrl;

            var response = _client.Execute(request);

            if (Verbose)
            {
                Console.WriteLine(response.Content);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Unable to authenticate. Please check your credentials.");
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                //if (!((request.Method == Method.DELETE || request.Method == Method.POST) && response.StatusCode == HttpStatusCode.NoContent))
                //    throw new InternalServerError(ExceptionDeserializer.Deserialize(response.Content));
            }
        }
        internal T Execute<T>(IRestRequest request)
            where T : new()
        {
            _client = Authenticator.Authenticate();
            _client.BaseUrl = BambooApi.BaseUrl;

            var response = _client.Execute<T>(request);

            if (Verbose)
            {
                Console.WriteLine(response.Content + Environment.NewLine);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Unable to authenticate. Please check your credentials.");
            }

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
