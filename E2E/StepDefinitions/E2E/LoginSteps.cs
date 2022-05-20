using System.Drawing;
using CoreAutomator.CommonUtils;
using CoreAutomator.ReportUtilities;
using E2E.Pages;
using CoreAutomator.Interfaces;

namespace E2E.StepDefinitions.E2E
{
    [Binding]
    public class LoginSteps
    {
        LoginPage loginPage;
        string? startupPath = Directory.GetCurrentDirectory();
        string? imageUrl = null;
        string? imageName = null;
        IWebActions iWebActions;

        public LoginSteps(ScenarioContext scenarioContext)
        {
            iWebActions = scenarioContext["iWebActions"] as IWebActions;
            loginPage = new LoginPage(iWebActions);
        }

        [When(@"I enter the username ""([^""]*)"" and password ""([^""]*)""")]
        public void WhenIEnterTheUsernameAndPassword(string userName, string password)
        {
            loginPage.EnterUserNameAndPassword(userName, password);
            GenerateReport.logStep = GenerateReport.logStep + "****** Username and Password entered ******";
        }

        [When(@"I click on login")]
        public void WhenIClickOnLogin()
        {
            loginPage.ClickLogin();
            GenerateReport.logStep = GenerateReport.logStep + "****** Clicked on Login button ******";
        }

        [Then(@"I user logged in to the application successfully")]
        public void ThenIUserLoggedInToTheApplicationSuccessfully()
        {
            loginPage.ValidateAfterLogin();
            GenerateReport.logStep = GenerateReport.logStep + "****** Login is successful *****";
        }

        [When(@"I download the actual image")]
        public void WhenIDownloadTheActualImage()
        {
            imageUrl = loginPage.GetImageUrl();
            imageName = loginPage.GetImageName();
            loginPage.DownloadImageFile(imageUrl, imageName, startupPath);
            GenerateReport.logStep = GenerateReport.logStep + "****** Downloaded the image file";
        }

        [Then(@"I compare the expected and actual images")]
        public void ThenICompareTheExpectedAndActualImages()
        {
            int count1 = 0;
            int count2 = 0;
            bool flag = Utils.CompareImages(new Bitmap(startupPath + "\\Google_Expected.png"), new Bitmap(startupPath + "\\" + imageName + ".png"), ref count1, ref count2);
            if (flag == false)
                GenerateReport.logStep = GenerateReport.logStep + " ***** Sorry, Images are not same , " + count2 + " wrong pixels found *****";
            else
                GenerateReport.logStep = GenerateReport.logStep + " ***** Images are same , " + count1 + " same pixels found and " + count2 + " wrong pixels found *****";
        }
    }
}