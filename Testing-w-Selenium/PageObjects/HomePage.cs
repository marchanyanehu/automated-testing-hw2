using OpenQA.Selenium;
using System;
using System.Linq;

namespace Testing_w_Selenium.PageObjects
{
    public class HomePage : BasePage
    {
        private By searchIcon = By.XPath("//*[@id='masthead']/div[1]/div/div[4]/div");
        private By searchBox = By.Name("s");
        private By searchResults = By.CssSelector(".search-result");
        private By aboutButton = By.XPath("//*[@id='menu-item-16178']/a");
        private By header = By.XPath("//h1[contains(text(), 'About')]");
        private By languageSwitcher = By.XPath("//*[@id='masthead']/div[1]/div/div[4]/ul");
        private By lithuanianSwitcher = By.XPath("//*[@id='masthead']/div[1]/div/div[4]/ul/li/ul/li[3]");
        private string contactsPageUrl = "https://en.ehu.lt/contacts/";
        
        // Selectors for Contact Form Submission Test
        private By admissionInquiries = By.XPath("//*[@id='post-17206']/div/table/tbody/tr[1]/th[1]");
        private By emailField = By.XPath("//*[@id='post-17206']/div/table/tbody/tr[2]/td[1]/a[1]");
        private By facebookField = By.XPath("//*[@id='post-17206']/div/table/tbody/tr[2]/td[1]/a[2]");
        private By phoneField = By.XPath("//*[@id='post-17206']/div/table/tbody/tr[2]/td[1]");

        public HomePage(IWebDriver driver) : base(driver) { }

        public void NavigateToHomePage(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public void ClickSearchIcon()
        {
            Driver.FindElement(searchIcon).Click();
        }

        public void EnterSearchTerm(string term)
        {
            Driver.FindElement(searchBox).SendKeys(term);
        }

        public void SubmitSearch()
        {
            Driver.FindElement(searchBox).SendKeys(Keys.Enter);
        }

        public bool SearchResultsContain(string keyword)
        {
            var results = Driver.FindElements(searchResults);
            return results.Any(result => result.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        public void ClickAboutButton()
        {
            Driver.FindElement(aboutButton).Click();
        }

        public IWebElement GetHeader()
        {
            return Driver.FindElement(header);
        }

        public void ClickLanguageSwitcher()
        {
            Driver.FindElement(languageSwitcher).Click();
        }

        public void SelectLithuanianLanguage()
        {
            Driver.FindElement(lithuanianSwitcher).Click();
        }

        public bool UrlContainsSearchTerm(string searchTerm)
        {
            return Driver.Url.Contains(searchTerm);
        }

        public void NavigateToContactsPage()
        {
            Driver.Navigate().GoToUrl(contactsPageUrl);
        }

        public string GetAdmissionInquiriesText()
        {
            return Driver.FindElement(admissionInquiries).Text;
        }

        public string GetEmailFieldText()
        {
            return Driver.FindElement(emailField).Text;
        }

        public string GetFacebookFieldText()
        {
            return Driver.FindElement(facebookField).Text;
        }

        public string GetPhoneFieldText()
        {
            return Driver.FindElement(phoneField).Text;
        }

        public IWebElement GetLithuanianHeader()
        {
            return Driver.FindElement(By.XPath("//*[@id='page']/footer/div/div/div[4]/div/div[3]/span[2]"));
        }

        public IWebDriver Driver => base.Driver;
    }
}