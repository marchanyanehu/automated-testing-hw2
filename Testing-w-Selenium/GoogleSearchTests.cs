using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;

namespace Testing_w_Selenium
{
    [TestFixture]
    public class SeleniumTests
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
        public void SearchForKeyword_ShouldContainKeywordInTitle()
        {
            driver.Navigate().GoToUrl("https://metanit.com/");

            // Find the search box and wait until it is clickable
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var magnifyingGlass = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("magnifying-glass")));
            magnifyingGlass.Click();
            var searchBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("q")));

            // Ensure the element is in the viewport and ready for interaction
            wait.Until(driver => searchBox.Displayed && searchBox.Enabled);

            // Enter the search term
            string searchTerm = "Python";
            searchBox.SendKeys(searchTerm);
            searchBox.Submit();

            string controlTerm = "Programmable Search Engine";

            // Wait for the results page to load and display the results
            wait.Until(drv => drv.Title.ToLower().Contains(controlTerm.ToLower()));

            // Verify the title contains the search term
            Assert.IsTrue(driver.Title.Contains(controlTerm), $"The title does not contain the control term: {controlTerm}");
        }

        [Test]
        public void VerifyNavigationToAboutEHUPage()
        {
            // Navigate to the EHU homepage
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            // Find the "About EHU" link and wait until it is clickable
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var aboutButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"menu-item-16178\"]/a")));
            aboutButton.Click();

            // Wait for the new page to load
            wait.Until(ExpectedConditions.UrlToBe("https://en.ehu.lt/about/"));

            // Verify the page title
            Assert.That(driver.Title, Is.EqualTo("About"), "The title does not match the expected value.");

            // Verify the content header
            var header = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[contains(text(), 'About')]")));
            Assert.IsTrue(header.Displayed, "The content header does not display the expected text.");
        }
        [Test]
        public void VerifySearchFunctionality()
        {
            // Navigate to the EHU homepage
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            // Find the search box and wait until it is clickable
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var searchIcon = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='masthead']/div[1]/div/div[4]/div")));
            searchIcon.Click();
            var searchBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("s")));

            // Enter the search term
            string searchTerm = "study programs";
            searchBox.SendKeys(searchTerm);
            searchBox.Submit();

            // Wait for the results page to load and display the results
            wait.Until(ExpectedConditions.UrlContains("/?s=study+programs"));

            // Verify the URL contains the search term
            Assert.IsTrue(driver.Url.Contains("/?s=study+programs"), "The URL does not contain the expected search term.");

            // Verify the search results include links to pages related to study programs
            var searchResults = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='page']/div[3]/div/div[2]")));
            Assert.IsTrue(searchResults.Text.Contains("study"), "The search results do not contain the expected text.");
        }
        [Test]
        public void VerifyLanguageChangeFunctionality()
        {
            // Navigate to the EHU homepage
            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            // Find the language switcher and wait until it is clickable
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var languageSwitcher = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='masthead']/div[1]/div/div[4]/ul")));
            languageSwitcher.Click();

            var lithuanianSwitcher = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='masthead']/div[1]/div/div[4]/ul/li/ul/li[3]")));
            lithuanianSwitcher.Click();
            // Wait for the new page to load
            wait.Until(ExpectedConditions.UrlToBe("https://lt.ehu.lt/"));

            // Verify the URL is the Lithuanian version of the site
            Assert.That(driver.Url, Is.EqualTo("https://lt.ehu.lt/"), "The URL does not match the expected Lithuanian version.");

            // Verify the page content appears in Lithuanian
            var header = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='page']/footer/div/div/div[4]/div/div[3]/span[2]")));
            Assert.IsTrue(header.Displayed, "The page content does not appear in Lithuanian.");
        }
        [Test]
        public void VerifyContactFormSubmission()
        {
            // Navigate to the contact page
            driver.Navigate().GoToUrl("https://en.ehu.lt/contacts/");

            // Find the form fields and wait until they are visible
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var admissionInquiries = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='post-17206']/div/table/tbody/tr[1]/th[1]")));
            var emailField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='post-17206']/div/table/tbody/tr[2]/td[1]/a[1]")));
            var facebookField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='post-17206']/div/table/tbody/tr[2]/td[1]/a[2]")));
            var phoneField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='post-17206']/div/table/tbody/tr[2]/td[1]")));

            // Verify the contact information
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