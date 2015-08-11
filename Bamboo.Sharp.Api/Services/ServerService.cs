using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bamboo.Sharp.Api.Services
{
    class ServerService : BaseService
    {
        public void GetServer()
        {
            RestRequest request = new RestRequest { Resource = "server ", Method = Method.GET };
            var r = Client.Execute<object>(request);
        }
        public void Pause()
        {
            RestRequest request = new RestRequest { Resource = "server/pause ", Method = Method.POST };
            var r = Client.Execute<object>(request);
        }
        public void PrepareForRestart()
        {
            RestRequest request = new RestRequest { Resource = "server/prepareForRestart ", Method = Method.PUT };
            var r = Client.Execute<object>(request);
        }
        public void Resume()
        {
            RestRequest request = new RestRequest { Resource = "server/resume ", Method = Method.POST };
            var r = Client.Execute<object>(request);
        }
    }
}
