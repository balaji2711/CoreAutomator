using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Net;

namespace CoreAutomator.Action
{
    public class RestActions
    {
        [ThreadStatic]
        public IRestClient? restClient;
        public IRestRequest? _restRequest;

        public void OpenRestClient(string baseURL)
        {
            restClient = new RestClient(baseURL);
        }

        public IRestRequest Get(string endPoint, string? accessToken = null)
        {
            _restRequest = new RestRequest(endPoint, Method.GET);
            _restRequest.AddHeader("Accept", "application/json");
            _restRequest.AddHeader("Authorization", "Bearer " + accessToken);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return _restRequest;
        }

        public IRestRequest Post(string endPoint, string requestBody)
        {
            _restRequest = new RestRequest(endPoint, Method.POST);
            _restRequest.AddHeader("Accept", "application/json");
            _restRequest.Parameters.Clear();
            _restRequest.AddParameter("application/json", requestBody, ParameterType.RequestBody);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return _restRequest;
        }

        public IRestRequest Put(string endPoint, string requestBody)
        {
            _restRequest = new RestRequest(endPoint, Method.PUT);
            _restRequest.AddHeader("Accept", "application/json");
            _restRequest.AddParameter("application/json", requestBody, ParameterType.RequestBody);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return _restRequest;
        }

        public IRestRequest Patch(string endPoint, string requestBody)
        {
            _restRequest = new RestRequest(endPoint, Method.PATCH);
            _restRequest.AddHeader("Accept", "application/json");
            _restRequest.AddParameter("application/json", requestBody, ParameterType.RequestBody);
            return _restRequest;
        }

        public IRestRequest Delete(string endPoint)
        {
            _restRequest = new RestRequest(endPoint, Method.DELETE);
            _restRequest.AddHeader("Accept", "application/json");
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return _restRequest;
        }
        public IRestResponse Execute(IRestRequest request)
        {
            return restClient.Execute(request);
        }

        public bool IsSuccess(HttpStatusCode responseCode)
        {
            var numericResponse = (int)responseCode;
            const int statusCodeOk = (int)HttpStatusCode.OK;
            const int statusCodeBadRequest = (int)HttpStatusCode.BadRequest;
            return numericResponse >= statusCodeOk && numericResponse < statusCodeBadRequest;
        }

        public static Dictionary<string, object> DeserializeResponse(IRestResponse restResponse)
        {
            var jsonObj = new JsonDeserializer().Deserialize<Dictionary<string, object>>(restResponse);
            return jsonObj;
        }

        public static string DeserializeResponseUsingJObject(IRestResponse restResponse, string responseObj)
        {
            var jObject = JObject.Parse(restResponse.Content.ToString());
            return jObject[responseObj]?.ToString();
        }
    }
}