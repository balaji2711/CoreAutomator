﻿using AventStack.ExtentReports;
using CoreAutomator.ClientFactory;
using CoreAutomator.CommonUtils;
using CoreAutomator.Interfaces;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using System.Drawing;
using System.Net;

namespace CoreAutomator.Action
{
    public enum LocatorType
    {
        Id,
        Name,
        XPath,
        LinkText,
        TagName,
        CssSelector,
        ClassName,
        PartialLinkText
    }

    public class SeleniumActions : DriverFactory, IWebActions
    {
        private IWebElement _element;
        IJavaScriptExecutor? javaScriptExecutor;

        public IWebActions Find(Locator locator)
        {
            switch (locator.Type)
            {
                case LocatorType.Id:
                    FindElement(By.Id(locator.Value));
                    break;

                case LocatorType.Name:
                    FindElement(By.Name(locator.Value));
                    break;

                case LocatorType.XPath:
                    FindElement(By.XPath(locator.Value));
                    break;

                case LocatorType.LinkText:
                    FindElement(By.LinkText(locator.Value));
                    break;

                case LocatorType.TagName:
                    FindElement(By.TagName(locator.Value));
                    break;

                case LocatorType.CssSelector:
                    FindElement(By.CssSelector(locator.Value));
                    break;

                case LocatorType.ClassName:
                    FindElement(By.ClassName(locator.Value));
                    break;

                case LocatorType.PartialLinkText:
                    FindElement(By.PartialLinkText(locator.Value));
                    break;

                default:
                    throw new ArgumentException($"Locator Type not yet implemented - {locator.Type}");
            }
            return this;
        }

        public IWebActions Find(Locator locator, int timeSpan)
        {
            switch (locator.Type)
            {
                case LocatorType.Id:
                    FindElement(By.Id(locator.Value));
                    WaitFor(10);
                    break;

                case LocatorType.Name:
                    FindElement(By.Name(locator.Value));
                    WaitFor(10);
                    break;

                case LocatorType.XPath:
                    FindElement(By.XPath(locator.Value));
                    WaitFor(10);
                    break;

                case LocatorType.LinkText:
                    FindElement(By.LinkText(locator.Value));
                    WaitFor(10);
                    break;

                case LocatorType.TagName:
                    FindElement(By.TagName(locator.Value));
                    WaitFor(10);
                    break;

                case LocatorType.CssSelector:
                    FindElement(By.CssSelector(locator.Value));
                    WaitFor(10);
                    break;

                case LocatorType.ClassName:
                    FindElement(By.ClassName(locator.Value));
                    WaitFor(10);
                    break;

                case LocatorType.PartialLinkText:
                    FindElement(By.PartialLinkText(locator.Value));
                    WaitFor(10);
                    break;

                default:
                    throw new ArgumentException($"Locator Type not yet implemented - {locator.Type}");
            }
            return this;
        }

        private void FindElement(By by)
        {
            try
            {
                _element = driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
            }
        }

        public IWebActions Clear()
        {
            try
            {
                _element.Clear();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to clear a text box - " + ex.Message);
            }
            return this;
        }

        public IWebActions Type(string value)
        {
            try
            {
                _element.SendKeys(value);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to enter text in the text box - " + ex.Message);
            }
            return this;
        }

        public IWebActions Click()
        {
            try
            {
                _element.Click();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Cannot click on the specified element - " + ex.Message);
            }
            return this;
        }

        public IWebActions SelectDropDown(string type, string? text = null, int index = 0)
        {
            try
            {
                SelectElement selectElement = new SelectElement(_element);
                if (type == "text")
                    selectElement.SelectByText(text);
                else if (type == "index")
                    selectElement.SelectByIndex(index);
                else
                    selectElement.SelectByValue(text);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Dropdown value selection - " + ex.Message);
            }
            return this;
        }

        public IWebActions AlertOk()
        {
            try
            {
                IAlert alertOk = driver.SwitchTo().Alert();
                alertOk.Accept();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to click Ok on alert - " + ex.Message);
            }
            return this;
        }

        public IWebActions AlertCancel()
        {
            try
            {
                IAlert alertCancel = driver.SwitchTo().Alert();
                alertCancel.Dismiss();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to click Ok on alert - " + ex.Message);
            }
            return this;
        }

        public IWebActions AlertText(string text)
        {
            try
            {
                IAlert alertText = driver.SwitchTo().Alert();
                alertText.SendKeys(text);
                alertText.Accept();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to click Ok on alert - " + ex.Message);
            }
            return this;
        }

        public IWebActions SwitchToFrame()
        {
            try
            {
                driver.SwitchTo().Frame(_element);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to switch to the frame - " + ex.Message);
            }
            return this;
        }

        public IWebActions SwitchToFrame(string frameName)
        {
            try
            {
                driver.SwitchTo().Frame(frameName);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to switch to the frame by framename - " + ex.Message);
            }
            return this;
        }

        public IWebActions SwitchToParentFrame()
        {
            try
            {
                driver.SwitchTo().ParentFrame();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to switch to parent frame - " + ex.Message);
            }
            return this;
        }

        public IWebActions SwitchToDefaultContent()
        {
            try
            {
                driver.SwitchTo().DefaultContent();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to switch to main content of page - " + ex.Message);
            }
            return this;
        }

        public string GetText()
        {
            string text = null;
            try
            {
                text = _element.Text;
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to get the text from web element - " + ex.Message.ToString());
            }
            return text;
        }

        public string GetInnerText()
        {
            string text = null;
            try
            {
                text = (string)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].innerHTML;", _element);
                text = text.Replace(Environment.NewLine, string.Empty);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to get the inner HTML from web element - " + ex.Message.ToString());
            }
            return text;
        }

        public int GetOpenWindowCount()
        {
            int count = 0;
            try
            {
                count = driver.WindowHandles.Count;
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to get a count of opened window - " + ex.Message.ToString());
            }
            return count;
        }

        public bool OpenNewTab(string url)
        {
            bool flag = false;
            try
            {
                javaScriptExecutor = (IJavaScriptExecutor)driver;
                javaScriptExecutor.ExecuteScript("window.open();");
                int count = GetOpenWindowCount();
                if (count >= 1)
                    SwitchToNewWindow();
                string script = "window.location =\'" + url + "\'";
                javaScriptExecutor.ExecuteScript(script);
                flag = true;
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to open a new tab - " + ex.Message.ToString());
            }
            return flag;
        }

        public IWebActions GoToURL(string url)
        {
            try
            {
                driver.Navigate().GoToUrl(url);
                WaitForSeconds(2);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to navigate to URL - " + ex.Message.ToString());
            }
            return this;
        }

        public string GetCurrentURL()
        {
            string url = null;
            try
            {
                url = driver.Url;
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to navigate to URL - " + ex.Message.ToString());
            }
            return url;
        }

        public IWebActions SwitchToWindowByTitle(string titleOfCurrentWindow)
        {
            try
            {
                List<string> windows = driver.WindowHandles.ToList();
                foreach (var window in windows)
                {
                    driver.SwitchTo().Window(window);
                    if (driver.Title == titleOfCurrentWindow)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Cannot switch to window by title - " + ex.Message);
            }
            return this;
        }

        public IWebActions SwitchToNewWindow()
        {
            try
            {
                string currentWindowHandler = driver.CurrentWindowHandle;
                WaitForSeconds(2);
                List<string> windows = driver.WindowHandles.ToList();
                windows.ForEach(t =>
                {
                    if (!t.Equals(currentWindowHandler))
                    {
                        driver.SwitchTo().Window(t);
                        string s = driver.Title;
                        driver.Manage().Window.Maximize();
                    }
                });
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to switch to new window - " + ex.Message.ToString());
            }
            return this;
        }

        public IWebActions WaitForSeconds(int seconds)
        {
            Thread.Sleep(seconds * 1000);
            return this;
        }

        public IWebActions ClickElementUsingJavaScript()
        {
            try
            {
                javaScriptExecutor = (IJavaScriptExecutor)driver;
                javaScriptExecutor.ExecuteScript("arguments[0].click();", _element);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Cannot click on the specified element - " + ex.Message);
            }
            return this;
        }

        public IWebActions DoubleClick()
        {
            try
            {
                Actions doubleClick = new Actions(driver);
                doubleClick.DoubleClick(_element).Build().Perform();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Cannot do the double click on the specified element - " + ex.Message);
            }
            return this;
        }

        public IWebActions MouseHover()
        {
            try
            {
                Actions mouseHover = new Actions(driver);
                mouseHover.MoveToElement(_element).Perform();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Cannot mouse hover on element - " + ex.Message);
            }
            return this;
        }

        public IWebActions MouseHoverClick()
        {
            try
            {
                Actions moveToElement = new Actions(driver);
                moveToElement.MoveToElement(_element).Click().Build().Perform();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Cannot move to the element - " + ex.Message);
            }
            return this;
        }

        public IWebActions ScrollToTopofThePage()
        {
            try
            {
                javaScriptExecutor = (IJavaScriptExecutor)driver;
                javaScriptExecutor.ExecuteScript("window.scrollTo(0, 0);");
                WaitForSeconds(2);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Cannot scroll to top of the page - " + ex.Message);
            }
            return this;
        }

        public IWebActions ScrollToBottomofThePage()
        {
            try
            {
                javaScriptExecutor = (IJavaScriptExecutor)driver;
                javaScriptExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Cannot scroll to bottom of the page - " + ex.Message);
            }
            return this;
        }

        public IWebActions ScrollToMiddleOfThePage()
        {
            try
            {
                javaScriptExecutor = (IJavaScriptExecutor)driver;
                javaScriptExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight/2);");
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Cannot scroll to middle of the page - " + ex.Message);
            }
            return this;
        }

        public IWebActions ScrollToElement()
        {
            try
            {
                javaScriptExecutor = (IJavaScriptExecutor)driver;
                javaScriptExecutor.ExecuteScript("arguments[0].scrollIntoView();", _element);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Cannot scroll to the element - " + ex.Message);
            }
            return this;
        }

        public IWebActions ScrollByPixels(int pixel)
        {
            try
            {
                javaScriptExecutor = (IJavaScriptExecutor)driver;
                javaScriptExecutor.ExecuteScript("window.scrollBy(0," + pixel + ")");
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Cannot scroll by pixel to the element - " + ex.Message);
            }
            return this;
        }

        public IWebActions ChangeResolution(int width, int height)
        {
            try
            {
                driver.Manage().Window.Size = new Size(width, height);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Cannot change the resolution - " + ex.Message);
            }
            return this;
        }

        public bool IsElementDisplay()
        {
            bool isDisplayed = false;
            try
            {
                isDisplayed = _element.Displayed;
                if (isDisplayed)
                    return isDisplayed;
                else
                    return isDisplayed;
            }
            catch { }
            return isDisplayed;
        }

        public void WaitForPageLoad()
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(60)).Until(
            d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public IWebActions WaitForEnabled(int timeSpan)
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                while (watch.Elapsed.Milliseconds < timeSpan)
                {
                    if (_element.Enabled)
                        break;
                }
            }
            catch (ElementNotInteractableException)
            {
            }
            return this;
        }

        public IWebActions WaitForVisible(int timeSpan)
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                while (watch.Elapsed.Milliseconds < timeSpan)
                {
                    if (_element.Displayed)
                        break;
                }
            }
            catch (ElementNotInteractableException)
            {
            }
            return this;
        }

        public IWebActions WaitFor(int timeSpan)
        {
            try
            {
                DefaultWait<IWebElement> fluentWait = new DefaultWait<IWebElement>(_element);
                fluentWait.Timeout = TimeSpan.FromSeconds(timeSpan);
                fluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
                fluentWait.Message = "Element to be searched not found";
                Func<IWebElement, bool> elementVisible = new Func<IWebElement, bool>((ele) =>
                {
                    if (ele.Displayed)
                        return true;
                    else
                        return false;
                });
                fluentWait.Until(elementVisible);
            }
            catch (ElementNotInteractableException)
            {
            }
            return this;
        }

        public string GetCssValue()
        {
            string? text = null;
            try
            {
                text = _element.GetCssValue("Color");
                return text;
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed: Unable to get a CSS value for an element - " + ex.Message.ToString());
            }
            return text;
        }

        public bool IsSortedDescending(List<long> list)
        {
            bool sorted = true;
            for (int i = 1; i < list.Count(); i++)
            {
                if (list[i - 1] >= list[i])
                    sorted = true;
                else
                {
                    sorted = false;
                    break;
                }
            }
            return sorted;
        }

        public bool IsSortedAscending(List<long> list)
        {
            bool sorted = true;
            for (int i = 1; i < list.Count(); i++)
            {
                if (list[i - 1] <= list[i])
                    sorted = true;
                else
                {
                    sorted = false;
                    break;
                }
            }
            return sorted;
        }

        public string GetCurrentWindowUrl()
        {
            string url = null;
            url = driver.Url;
            return url;
        }

        public string GetAttribute(string type)
        {
            string? text = null;
            try
            {
                text = _element.GetAttribute(type);
                return text;
            }
            catch (Exception ex)
            {
                Assert.Fail("Unable to get the attribute for " + type + " from element - " + ex.Message.ToString());
            }
            return text;
        }

        public void DownloadImageFile(string imageUrl, string imageName, string path)
        {
            WebClient downloadFile = new WebClient();
            downloadFile.DownloadFile(imageUrl, path + "\\" + imageName + ".png");
        }

        public static MediaEntityModelProvider CaptureScreenShot(string name)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, name).Build();
        }

        public void CloseBrowser()
        {
            QuitBrowser();
        }

        public IWebActions OpenBrowser(string browserName, string webBaseUrl, string headlessExecution)
        {
            base.OpenBrowser(browserName, webBaseUrl, headlessExecution);
            return this;
        }
    }
}