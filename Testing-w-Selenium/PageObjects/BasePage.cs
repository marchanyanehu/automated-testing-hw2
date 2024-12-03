using OpenQA.Selenium;

namespace Testing_w_Selenium.PageObjects
{
    public abstract class BasePage
    {
        protected IWebDriver Driver;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
        }
    }
}