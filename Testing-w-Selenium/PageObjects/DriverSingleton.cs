using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
using Testing_w_Selenium.Configuration;

namespace Testing_w_Selenium.PageObjects
{
    public sealed class DriverSingleton
    {
        private static IWebDriver _driver;
        private static readonly object _lock = new object();

        static DriverSingleton()
        {
            var framework = DetectTestFramework();
            LoggerConfig.ConfigureLogger(framework);
        }

        private static TestFramework DetectTestFramework()
        {
            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();
            
            foreach (var frame in frames)
            {
                var assemblyName = frame.GetMethod().Module.Assembly.GetName().Name;
                if (assemblyName.Contains("NUnit"))
                    return TestFramework.NUnit;
                if (assemblyName.Contains("xUnit"))
                    return TestFramework.XUnit;
            }

            Log.Warning("Could not determine test framework, defaulting to XUnit");
            return TestFramework.XUnit;
        }

        private DriverSingleton() { }

        public static IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                lock (_lock)
                {
                    if (_driver == null)
                    {
                        try
                        {
                            var options = new ChromeOptions();
                            _driver = new ChromeDriver(options);
                            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                            _driver.Manage().Window.Maximize();
                            Log.Debug("WebDriver instance created.");
                        }
                        catch (Exception ex)
                        {
                            LoggerConfig.LogFatal(ex, "Failed to initialize WebDriver. Application cannot continue.");
                            throw;
                        }
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
                Log.Debug("WebDriver instance disposed.");
                LoggerConfig.CloseAndFlush();
            }
        }
    }
}