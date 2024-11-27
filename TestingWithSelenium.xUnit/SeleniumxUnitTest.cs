using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, MaxParallelThreads = 16)]

namespace TestingWithSelenium.xUnit
{
    [Trait("Category", "Navigation")]
    public class NavigationTests : IDisposable
    {
        private readonly IWebDriver driver;

        public NavigationTests()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();
        }

        [Theory]
        [InlineData("https://en.ehu.lt/", "About")]
        [InlineData("https://en.ehu.lt/about/", "About")]
        public void VerifyNavigationToAboutPage(string url, string expectedTitle)
        {
            driver.Navigate().GoToUrl(url);
            var aboutButton = driver.FindElement(By.XPath("//*[@id=\"menu-item-16178\"]/a"));
            aboutButton.Click();
            Assert.Equal("https://en.ehu.lt/about/", driver.Url);
            Assert.Equal(expectedTitle, driver.Title);
            var header = driver.FindElement(By.TagName("h1"));
            Assert.Equal("About", header.Text);
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }

    [Trait("Category", "Search")]
    public class SearchTests : IDisposable
    {
        private readonly IWebDriver driver;

        public SearchTests()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();
        }

        [Theory]
        [InlineData("study programs", "study program")]
        public void VerifySearchFunctionalityWithDifferentQueries(string query, string expectedText)
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            var searchButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div"));
            searchButton.Click();
            var searchBar = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/div/form/div/input"));
            searchBar.SendKeys(query);
            searchBar.SendKeys(Keys.Enter);

            Assert.Contains($"/?s={query.Replace(" ", "+")}", driver.Url);
            var searchResults = driver.FindElements(By.XPath("//*[@id=\"page\"]/div[3]"));
            bool resultsContainSearchTerm = searchResults.Any(result => result.Text.Contains(expectedText, StringComparison.OrdinalIgnoreCase));
            Assert.True(resultsContainSearchTerm, $"Search results do not contain any expected text: {expectedText}");
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }

    [Trait("Category", "LanguageSwitch")]
    public class LanguageSwitchTests : IDisposable
    {
        private readonly IWebDriver driver;

        public LanguageSwitchTests()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();
        }

        [Fact]
        public void VerifyLanguageSwitchFunctionality()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");
            var languageSwitchButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul"));
            languageSwitchButton.Click();
            var ltButton = driver.FindElement(By.XPath("//*[@id=\"masthead\"]/div[1]/div/div[4]/ul/li/ul/li[3]/a"));
            ltButton.Click();
            Assert.Equal("https://lt.ehu.lt/", driver.Url);
            var htmlTag = driver.FindElement(By.TagName("html"));
            string langAttribute = htmlTag.GetAttribute("lang");
            Assert.Equal("lt-LT", langAttribute);
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}