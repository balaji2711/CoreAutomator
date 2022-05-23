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
            try
            {
                iWebActions.Find(txtUsername).Clear().Type(userName);
                iWebActions.Find(txtPassword).Clear().Type(password);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to enter a text into textbox - " + ex.Message.ToString());
            }
        }

        public void ClickLogin()
        {
            try
            {
                iWebActions.Find(btnLogin).Click();
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to click a login button - " + ex.Message.ToString());
            }
        }

        public void ValidateAfterLogin()
        {
            string? text = null;
            try
            {
                text = iWebActions.Find(title,10).GetText();
                if (text != null)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.IsNotNull(text, "Login is not successful\n");
                        Assert.AreEqual("PRODUCTS", text, "Login is not successful");
                    });
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to fetch a header text in home page - " + ex.Message.ToString());
            }
        }

        public string GetImageUrl()
        {
            string? text = null;
            try
            {
                text = iWebActions.Find(image).GetAttribute("src");
                return text;
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to fetch the image Url - " + ex.Message.ToString());
            }
            return text;
        }

        public string GetImageName()
        {
            string? text = null;
            try
            {
                text = iWebActions.Find(image).GetAttribute("alt");
                return text;
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to fetch the image alternate text - " + ex.Message.ToString());
            }
            return text;
        }

        public void DownloadImageFile(string imageUrl, string imageName, string path)
        {
            try
            {
                iWebActions.DownloadImageFile(imageUrl, imageName, path);
            }
            catch (Exception ex)
            {
                Assert.Fail("Downloaded the image file - " + ex.Message.ToString());
            }
        }
    }
}