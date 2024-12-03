using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace Testing_w_Selenium.PageObjects
{
    public sealed class DriverSingleton
    {
        private static IWebDriver _driver;
        private static readonly object _lock = new object();

        private DriverSingleton() { }

        public static IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                lock (_lock)
                {
                    if (_driver == null)
                    {
                        var options = new ChromeOptions();
                        _driver = new ChromeDriver(options);
                        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                        _driver.Manage().Window.Maximize();
                    }
                }
            }
            return _driver;
        }

        public static void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver = null;
            }
        }
    }
}