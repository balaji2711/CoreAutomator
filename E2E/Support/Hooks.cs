using CoreAutomator.Action;
using CoreAutomator.CommonUtils;
using CoreAutomator.Interfaces;
using CoreAutomator.ReportUtilities;

namespace E2E.Support
{
    [Binding]
    public class Hooks
    {
        IWebActions iWebActions;
        static string? browserName = null;
        static string? webBaseUrl = null;
        static string? headlessMode = null;
        static string? baseUrl = null;
        static string? webSocketUrl = null;
        static string? emailIDs = null;
        static string? author = null;
        static string? suiteName = null;
        static string? reportName = null;
        static bool rerun = false;

        [BeforeTestRun]
        [Obsolete]
        public static void InitializeTestSuite()
        {
            ConfigManager.InitializeEnvConfig();
            browserName = Environment.GetEnvironmentVariable("Browser") ?? ConfigManager.config.testConfig.browser;
            webBaseUrl = Environment.GetEnvironmentVariable("WebBaseUrl") ?? ConfigManager.config.appConfig.webBaseURL;
            headlessMode = Environment.GetEnvironmentVariable("HeadlessMode") ?? ConfigManager.config.testConfig.headlessMode;
            baseUrl = Environment.GetEnvironmentVariable("BaseUrl") ?? ConfigManager.config.appConfig.baseURL;
            webSocketUrl = Environment.GetEnvironmentVariable("WebSocketUrl") ?? ConfigManager.config.appConfig.socketType + ConfigManager.config.appConfig.serviceFabricURL;
            emailIDs = Environment.GetEnvironmentVariable("EmailIDs") ?? ConfigManager.config.testConfig.emailIDs;
            author = Environment.GetEnvironmentVariable("Author") ?? ConfigManager.config.testConfig.author;
            suiteName = Environment.GetEnvironmentVariable("Suite") ?? ConfigManager.config.testConfig.suite;
            reportName = Environment.GetEnvironmentVariable("Report") ?? ConfigManager.config.testConfig.reportName;
            rerun = Convert.ToBoolean(Environment.GetEnvironmentVariable("Rerun"));
            GenerateReport.InitializeTestSuite(browserName, webBaseUrl, baseUrl, webSocketUrl, author, reportName);
        }

        [AfterTestRun]
        public static void EndTestSuite()
        {
            GenerateReport.EndTestSuite(emailIDs, suiteName, rerun);
        }

        //If you choose to have a single browser instance for all tests,
        //you can replace these attributes with[BeforeTestRun] and[AfterTestRun]

        [Before]
        [Obsolete]
        public void InitializeDrivers(ScenarioContext scenarioContext)
        {
            GenerateReport.logStep = null;
            GenerateReport.totalCount++;
            iWebActions = new SeleniumActions();
            iWebActions.OpenBrowser(browserName, webBaseUrl, headlessMode);
            GenerateReport.Before(scenarioContext);
            scenarioContext["iWebActions"] = iWebActions;
        }

        [After]
        [Obsolete]
        public void CloseDrivers(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            if (scenarioContext.TestError == null)
                GenerateReport.passCount++;
            else if (scenarioContext.TestError != null)
            {
                GenerateReport.failCount++;
                GenerateReport.AddFailedFeature(featureContext);
            }
            iWebActions.CloseBrowser();
        }

        [BeforeFeature]
        [Obsolete]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            GenerateReport.BeforeFeature(featureContext);
        }

        [AfterFeature]
        [Obsolete]
        public static void AfterFeature(FeatureContext featureContext)
        {
        }

        [AfterStep]
        [Obsolete]
        public void CreateReportingSteps(ScenarioContext scenarioContext)
        {
            GenerateReport.AfterStep(scenarioContext);
        }
    }
}