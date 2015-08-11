using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bamboo.Sharp.Api.Services
{
    class ElasticConfiguration : BaseService
    {
        public void SetElasticConfigurationImageId(string imageId)
        {
            RestRequest request = new RestRequest { Resource = "elasticConfiguration/image-id/{imageId} ", Method = Method.PUT };
            request.AddParameter("imageId", imageId, ParameterType.UrlSegment);
            var r = Client.Execute<object>(request);
        }
        public void GetElasticConfiguration()
        {
            RestRequest request = new RestRequest
            {
                Resource = "elasticConfiguration ",
                Method = Method.GET
            };
            var r = Client.Execute<object>(request);
        }
        public void SetElasticConfiguration()
        {
            RestRequest request = new RestRequest
            {
                Resource = "elasticConfiguration ",
                Method = Method.POST
            };
            var r = Client.Execute<object>(request);
        }
        public void GetConfigurationId(int configurationId)
        {
            RestRequest request = new RestRequest
            {
                Resource = "elasticConfiguration/{configurationId}",
                Method = Method.GET
            };
            request.AddParameter("configurationId", configurationId, ParameterType.UrlSegment);
            var r = Client.Execute<object>(request);
        }

        public void AddConfiguration(int configurationId)
        {
            RestRequest request = new RestRequest
            {
                Resource = "elasticConfiguration/{configurationId}",
                Method = Method.PUT
            };
            request.AddParameter("configurationId", configurationId, ParameterType.UrlSegment);
            var r = Client.Execute<object>(request);
        }
        public void RemoveConfigurationId(int configurationId)
        {
            RestRequest request = new RestRequest
            {
                Resource = "elasticConfiguration/{configurationId}",
                Method = Method.DELETE
            };
            request.AddParameter("configurationId", configurationId, ParameterType.UrlSegment);
            var r = Client.Execute<object>(request);
        }
        public void GetConfigurationName(string configurationName)
        {
            RestRequest request = new RestRequest
            {
                Resource = "elasticConfiguration/{configurationName}",
                Method = Method.GET
            };
            request.AddParameter("configurationName", configurationName, ParameterType.UrlSegment);
            var r = Client.Execute<object>(request);
        }

        public void AddConfigurationName(string configurationName)
        {
            RestRequest request = new RestRequest
            {
                Resource = "elasticConfiguration/{configurationName}",
                Method = Method.PUT
            };
            request.AddParameter("configurationName", configurationName, ParameterType.UrlSegment);
            var r = Client.Execute<object>(request);
        }
        public void RemoveConfigurationName(string configurationName)
        {
            RestRequest request = new RestRequest
            {
                Resource = "elasticConfiguration/{configurationName}",
                Method = Method.DELETE
            };
            request.AddParameter("configurationName", configurationName, ParameterType.UrlSegment);
            var r = Client.Execute<object>(request);
        }
    }
}
