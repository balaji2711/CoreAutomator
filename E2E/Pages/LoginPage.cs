using CoreAutomator.Action;
using CoreAutomator.CommonUtils;
using CoreAutomator.Interfaces;
using NUnit.Framework;
namespace E2E.Pages
{
    public class LoginPage
    {
        IWebActions iWebActions;
        Locator txtUsername = new(LocatorType.Id, "user-name");
        Locator txtPassword = new(LocatorType.Id, "password");
        Locator btnLogin = new(LocatorType.Id, "login-button");
        Locator title = new(LocatorType.XPath, "//span[@class='title']");
        Locator image = new(LocatorType.XPath, "//div/img");

        public LoginPage(IWebActions iWebActions)
        {
            this.iWebActions = iWebActions;
        }

        public void EnterUserNameAndPassword(string userName, string password)
        {
            iWebActions.Find(txtUsername).Clear().Type(userName);
            iWebActions.Find(txtPassword).Clear().Type(password);
        }

        public void ClickLogin()
        {
            iWebActions.Find(btnLogin).Click();
        }

        public void ValidateAfterLogin()
        {
            string? text = null;
            text = iWebActions.Find(title, 10).GetText();
            if (text != null)
            {
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(text, "Login is not successful\n");
                    Assert.AreEqual("PRODUCTS", text, "Login is not successful");
                });
            }
        }

        public string GetImageUrl()
        {
            string? text = null;
            text = iWebActions.Find(image).GetAttribute("src");
            return text;
        }

        public string GetImageName()
        {
            string? text = null;
            text = iWebActions.Find(image).GetAttribute("alt");
            return text;
        }

        public void DownloadImageFile(string imageUrl, string imageName, string path)
        {
            iWebActions.DownloadImageFile(imageUrl, imageName, path);
        }
    }
}