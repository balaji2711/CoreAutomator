using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Net;

namespace CoreAutomator.Action
{
    public enum ContentType
    {
        TEXT,
        JSON,
        XML,
        NONE
    }

    public class RestActions
    {
        [ThreadStatic]
        public IRestClient? restClient;
        private IRestRequest? _restRequest;
        private string _url;
        private ContentType _contentType = ContentType.NONE;

        public IRestRequest Send()
        {
            return _restRequest;
        }

        public RestActions SetRequest(Method method, string path)
        {
            _url = path;
            _restRequest = new RestRequest(_url, method);
            return this;
        }

        public RestActions AddHeader(string name, string value)
        {
            _restRequest.AddHeader(name, value);
            return this;
        }

        public RestActions SetContentType(ContentType contentType)
        {
            string contentTypeStr = GetContentType(contentType);
            if (contentTypeStr != "none")
            {
                _contentType = contentType;
                AddHeader("Accept", contentTypeStr);
                AddHeader("Content-Type", contentTypeStr);
            }
            return this;
        }

        private string GetContentType(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.TEXT: return "text/plain";
                case ContentType.JSON: return "application/json";
                case ContentType.XML: return "application/xml";
                default: return "none";
            }
        }

        public void OpenRestClient(string baseURL)
        {
            restClient = new RestClient(baseURL);
        }

        public RestActions Body(string requestBody)
        {
            _restRequest.Parameters.Clear();
            _restRequest.AddParameter(GetContentType(_contentType), requestBody, ParameterType.RequestBody);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return this;
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

        public static Dictionary<string, Object> DeserializeResponse(IRestResponse restResponse)
        {
            var jsonObj = new JsonDeserializer().Deserialize<Dictionary<string, Object>>(restResponse);
            return jsonObj;
        }

        public static string DeserializeResponseUsingJObject(IRestResponse restResponse, string responseObj)
        {
            var jObject = JObject.Parse(restResponse.Content.ToString());
            return jObject[responseObj]?.ToString();
        }
    }
}