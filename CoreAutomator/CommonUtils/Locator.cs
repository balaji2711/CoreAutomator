using CoreAutomator.Action;

namespace CoreAutomator.CommonUtils
{
    public class Locator
    {
        public LocatorType Type;
        public string Value;
        public Locator(LocatorType LocatorType, string LocatorValue)
        {
            Type = LocatorType;
            Value = LocatorValue;
        }
    }
}