namespace CoreAutomator.CommonUtils
{
    public static class ConfigManager
    {
        static string startupPath = Directory.GetCurrentDirectory();
        public static dynamic config = null;

        public static void InitializeEnvConfig()
        {
            config = Utils.JsonParser(startupPath + "\\Resources\\config.json");
        }
    }
}