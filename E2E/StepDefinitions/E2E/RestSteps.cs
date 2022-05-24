using NUnit.Framework;
using RestSharp;
using System.Net;
using CoreAutomator.CommonUtils;
using CoreAutomator.ReportUtilities;
using CoreAutomator.Action;

namespace E2E.StepDefinitions.E2E
{
    [Binding]
    public class RestSteps
    {
        private IRestRequest restRequest;
        private IRestResponse restResponse;
        HttpStatusCode statuscode;
        private int _numericResponse = 0;
        RestActions restActions;
        string baseUrl = System.Environment.GetEnvironmentVariable("BaseUrl") ?? Utils.RestBaseUrl();

        public RestSteps()
        {
            restActions = new RestActions();
            restActions.OpenRestClient(baseUrl);
        }

        [When(@"I run the user request API")]
        public void WhenIRunTheUserRequestAPI()
        {
            restRequest = restActions.SetRequest(Method.GET, "api/users?page=2").Send();
            restResponse = restActions.Execute(restRequest);
            statuscode = restResponse.StatusCode;
            _numericResponse = (int)statuscode;
            GenerateReport.logStep = GenerateReport.logStep + " ****** API Status Code is ******" + _numericResponse;
            GenerateReport.logStep = GenerateReport.logStep + " ****** API Reponse is ******" + restResponse.Content.ToString();
        }

        [Then(@"verify the success response from user API")]
        public void ThenVerifyTheSuccessResponseFromUserAPI()
        {
            if (restActions.IsSuccess(statuscode))
            {
                if (restResponse.Content.ToString().Contains("email") && restResponse.Content.ToString().Contains("first_name") && restResponse.Content.ToString().Contains("last_name"))
                    GenerateReport.logStep = GenerateReport.logStep + " ****** Attributes are available ******";
            }
            else
                Assert.Fail(" ****** Get request is failed and it's throwing an error with status code as - " + _numericResponse);
        }

        [When(@"I run the post call")]
        public void WhenIRunThePostCall()
        {
            restRequest = restActions.SetRequest(Method.POST, "api/users")
                                     .SetContentType(ContentType.JSON)
                                     .Body(@"{""name"":""morpheus"",""job"":""leader""}")
                                     .Send();
            restResponse = restActions.Execute(restRequest);
            statuscode = restResponse.StatusCode;
            _numericResponse = (int)statuscode;
            GenerateReport.logStep = GenerateReport.logStep + " ****** API Status Code is ******" + _numericResponse;
            GenerateReport.logStep = GenerateReport.logStep + " ****** API Reponse is ******" + restResponse.Content.ToString();
        }

        [Then(@"verify the response from post call")]
        public void ThenVerifyTheResponseFromPostCall()
        {
            if (restActions.IsSuccess(statuscode))
            {
                var result = RestActions.DeserializeResponse(restResponse);
                Assert.That(result["name"], Is.EqualTo("morpheus"));
                GenerateReport.logStep = GenerateReport.logStep + " ****** Name is matching ******";
            }
            else
                Assert.Fail(" ****** Post request is failed and it's throwing an error with status code as - " + _numericResponse);
        }

        [When(@"I run the put call")]
        public void WhenIRunThePutCall()
        {
            restRequest = restActions.SetRequest(Method.PUT, "api/users/2")
                                     .SetContentType(ContentType.JSON)
                                     .Body(@"{""name"":""morpheus"",""job"":""zionresident""}")
                                     .Send();
            restResponse = restActions.Execute(restRequest);
            statuscode = restResponse.StatusCode;
            _numericResponse = (int)statuscode;
            GenerateReport.logStep = GenerateReport.logStep + " ****** API Status Code is ******" + _numericResponse;
            GenerateReport.logStep = GenerateReport.logStep + " ****** API Reponse is ******" + restResponse.Content.ToString();
        }

        [Then(@"verify the response from put call")]
        public void ThenVerifyTheResponseFromPutCall()
        {
            if (restActions.IsSuccess(statuscode))
            {
                var result = RestActions.DeserializeResponse(restResponse);
                Assert.That(result["job"], Is.EqualTo("zionresident"));
                GenerateReport.logStep = GenerateReport.logStep + " ****** Job is updated ******";
            }
            else
                Assert.Fail(" ****** Put request is failed and it's throwing an error with status code as - " + _numericResponse);
        }

        [When(@"I run the patch call")]
        public void WhenIRunThePatchCall()
        {
            restRequest = restActions.SetRequest(Method.PATCH, "api/users/2")
                                     .SetContentType(ContentType.JSON)
                                     .Body(@"{""name"":""morpheus"",""job"":""zionresident""}")
                                     .Send();
            restResponse = restActions.Execute(restRequest);
            statuscode = restResponse.StatusCode;
            _numericResponse = (int)statuscode;
            GenerateReport.logStep = GenerateReport.logStep + " ****** API Status Code is ******" + _numericResponse;
            GenerateReport.logStep = GenerateReport.logStep + " ****** API Reponse is ******" + restResponse.Content.ToString();
        }

        [Then(@"verify the response from patch call")]
        public void ThenVerifyTheResponseFromPatchCall()
        {
            if (restActions.IsSuccess(statuscode))
            {
                var result = RestActions.DeserializeResponse(restResponse);
                Assert.That(result["job"], Is.EqualTo("zionresident"));
                GenerateReport.logStep = GenerateReport.logStep + " ****** Job is updated ******";
            }
            else
                Assert.Fail(" ****** Patch request is failed and it's throwing an error with status code as - " + _numericResponse);
        }

        [When(@"I run the delete call")]
        public void WhenIRunTheDeleteCall()
        {
            restRequest = restActions.SetRequest(Method.DELETE, "api/users/2").Send();
            restResponse = restActions.Execute(restRequest);
            statuscode = restResponse.StatusCode;
            _numericResponse = (int)statuscode;
            GenerateReport.logStep = GenerateReport.logStep + " ****** API Status Code is ******" + _numericResponse;
        }

        [Then(@"verify the response from delete call")]
        public void ThenVerifyTheResponseFromDeleteCall()
        {
            if (_numericResponse == 204)
                GenerateReport.logStep = GenerateReport.logStep + " ****** Record is deleted ******";
            else
                Assert.Fail("Delete request response code is not 204 and the actual code is - " + _numericResponse);
        }
    }
}