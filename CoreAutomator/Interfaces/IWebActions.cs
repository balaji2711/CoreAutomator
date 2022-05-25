using CoreAutomator.CommonUtils;

namespace CoreAutomator.Interfaces
{
    public interface IWebActions
    {
        IWebActions AlertCancel();
        IWebActions AlertOk();
        IWebActions AlertText(string text);
        IWebActions ChangeResolution(int width, int height);
        IWebActions Clear();
        IWebActions Click();
        IWebActions ClickElementUsingJavaScript();
        IWebActions DoubleClick();
        void DownloadImageFile(string imageUrl, string imageName, string path);
        IWebActions Find(Locator locator);
        IWebActions Find(Locator locator, int timeInSeconds);

        IWebActions WaitForUrl(int timeInSeconds, string searchText);
        IWebActions WaitForTitle(int timeInSeconds, string searchText, bool matchExact = false);
        IWebActions WaitForText(Locator locator, int timeInSeconds, string searchText, bool matchExact = false);
        string GetAttribute(string type);
        string GetCssValue();
        string GetCurrentURL();
        string GetCurrentWindowUrl();
        string GetInnerText();
        int GetOpenWindowCount();
        string GetText();
        IWebActions GoToURL(string url);
        bool IsElementDisplay();
        bool IsSortedAscending(List<long> list);
        bool IsSortedDescending(List<long> list);
        IWebActions MouseHover();
        IWebActions MouseHoverClick();
        bool OpenNewTab(string url);
        IWebActions ScrollByPixels(int pixel);
        IWebActions ScrollToBottomofThePage();
        IWebActions ScrollToElement();
        IWebActions ScrollToMiddleOfThePage();
        IWebActions ScrollToTopofThePage();
        IWebActions SelectDropDown(string type, string? text = null, int index = 0);
        IWebActions SwitchToDefaultContent();
        IWebActions SwitchToFrame();
        IWebActions SwitchToFrame(string frameName);
        IWebActions SwitchToNewWindow();
        IWebActions SwitchToParentFrame();
        IWebActions SwitchToWindowByTitle(string titleOfCurrentWindow);
        IWebActions Type(string value);
        IWebActions WaitForEnabled(int timeInSeconds);
        void WaitForPageLoad();
        IWebActions WaitFor(int timeInSeconds);
        IWebActions WaitForVisible(int timeInSeconds);
        IWebActions WaitForAngularRequestToFinish();
        IWebActions Refresh();
        void CloseBrowser();
        IWebActions OpenBrowser(string browserName, string webBaseUrl, string headlessExecution);
    }
}