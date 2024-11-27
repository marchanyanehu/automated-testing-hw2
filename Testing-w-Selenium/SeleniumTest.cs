using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;

namespace Testing_w_Selenium
{
    [TestFixture]
    public class SeleniumTest
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void VerifyNavigationToAboutEHUPage()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var aboutButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"menu-item-16178\"]/a")));
            aboutButton.Click();

            wait.Until(ExpectedConditions.UrlToBe("https://en.ehu.lt/about/"));

            Assert.That(driver.Title, Is.EqualTo("About"), "The title does not match the expected value.");

            var header = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[contains(text(), 'About')]")));
            Assert.IsTrue(header.Displayed, "The content header does not display the expected text.");
        }

        [Test]
        public void VerifySearchFunctionality()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var searchIcon = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='masthead']/div[1]/div/div[4]/div")));
            searchIcon.Click();
            var searchBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("s")));

            string searchTerm = "study programs";
            searchBox.SendKeys(searchTerm);
            searchBox.Submit();

            wait.Until(ExpectedConditions.UrlContains("/?s=study+programs"));

            Assert.IsTrue(driver.Url.Contains("/?s=study+programs"), "The URL does not contain the expected search term.");

            var searchResults = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='page']/div[3]/div/div[2]")));
            Assert.IsTrue(searchResults.Text.Contains("study"), "The search results do not contain the expected text.");
        }

        [Test]
        public void VerifyLanguageChangeFunctionality()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var languageSwitcher = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='masthead']/div[1]/div/div[4]/ul")));
            languageSwitcher.Click();

            var lithuanianSwitcher = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='masthead']/div[1]/div/div[4]/ul/li/ul/li[3]")));
            lithuanianSwitcher.Click();

            wait.Until(ExpectedConditions.UrlToBe("https://lt.ehu.lt/"));

            Assert.That(driver.Url, Is.EqualTo("https://lt.ehu.lt/"), "The URL does not match the expected Lithuanian version.");

            var header = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='page']/footer/div/div/div[4]/div/div[3]/span[2]")));
            Assert.IsTrue(header.Displayed, "The page content does not appear in Lithuanian.");
        }

        [Test]
        public void VerifyContactFormSubmission()
        {
            driver.Navigate().GoToUrl("https://en.ehu.lt/contacts/");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var admissionInquiries = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='post-17206']/div/table/tbody/tr[1]/th[1]")));
            var emailField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='post-17206']/div/table/tbody/tr[2]/td[1]/a[1]")));
            var facebookField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='post-17206']/div/table/tbody/tr[2]/td[1]/a[2]")));
            var phoneField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='post-17206']/div/table/tbody/tr[2]/td[1]")));

            Assert.AreEqual("Admission inquiries", admissionInquiries.Text, "The admission inquiries text does not match.");
            Assert.AreEqual("recruitment@ehu.lt", emailField.Text, "The email field text does not match.");
            Assert.AreEqual("European Humanities University", facebookField.Text, "The Facebook field text does not match.");
            Assert.IsTrue(phoneField.Text.Contains("+370 (644) 96 317"), "The phone field text does not match.");
        }

        [TearDown]
        public void TearDown()
        {
            if (driver == null)
            {
                return;
            }
            driver.Quit();
        }
    }
}