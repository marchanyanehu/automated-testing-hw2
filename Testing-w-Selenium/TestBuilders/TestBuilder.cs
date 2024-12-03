using Testing_w_Selenium.PageObjects;

namespace Testing_w_Selenium.TestBuilders
{
    public class TestBuilder
    {
        private HomePage _homePage;

        public TestBuilder() { }

        public TestBuilder InitializeDriver()
        {
            _homePage = new HomePage(DriverSingleton.GetDriver());
            return this;
        }

        public HomePage BuildHomePage()
        {
            return _homePage;
        }

        public void TearDown()
        {
            DriverSingleton.QuitDriver();
        }
    }
}