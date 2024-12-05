using Testing_w_Selenium.PageObjects;
using Serilog;

namespace Testing_w_Selenium.TestBuilders
{
    public class TestBuilder
    {
        private HomePage _homePage;

        public TestBuilder() { }

        public TestBuilder InitializeDriver()
        {
            _homePage = new HomePage(DriverSingleton.GetDriver());
            Log.Debug("Driver initialized in TestBuilder.");
            return this;
        }

        public HomePage BuildHomePage()
        {
            return _homePage;
        }

        public void TearDown()
        {
            Log.Debug("TearDown started in TestBuilder.");
            DriverSingleton.QuitDriver();
            Log.Debug("TearDown completed in TestBuilder.");
        }
    }
}