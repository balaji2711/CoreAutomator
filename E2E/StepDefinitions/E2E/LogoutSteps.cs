using CoreAutomator.Interfaces;
using CoreAutomator.ReportUtilities;
using E2E.Pages;

namespace E2E.StepDefinitions.E2E
{
    [Binding]
    public class LogoutSteps
    {
        LoginPage loginPage;
        IWebActions iWebActions;

        public LogoutSteps(ScenarioContext scenarioContext)
        {
            iWebActions = scenarioContext["iWebActions"] as IWebActions;
            loginPage = new LoginPage(iWebActions);
        }


        [When(@"I click on logout")]
        public void WhenIClickOnLogout()
        {
            loginPage.ClickLogin();
            GenerateReport.logStep = GenerateReport.logStep + "****** Clicked on Login button ******";
        }

        [Then(@"I should see user logged out from application successfully")]
        public void ThenIShouldSeeUserLoggedOutFromApplicationSuccessfully()
        {
            loginPage.ValidateAfterLogin();
            GenerateReport.logStep = GenerateReport.logStep + "****** Login is successful *****";
        }
    }
}