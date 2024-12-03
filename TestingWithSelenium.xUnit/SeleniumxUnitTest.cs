using System;
using Xunit;
using Testing_w_Selenium.TestBuilders;
using Testing_w_Selenium.PageObjects;

namespace TestingWithSelenium.xUnit
{
    public class SeleniumxUnitTest : IDisposable
    {
        private readonly TestBuilder _testBuilder;
        private readonly HomePage _homePage;

        public SeleniumxUnitTest()
        {
            _testBuilder = new TestBuilder().InitializeDriver();
            _homePage = _testBuilder.BuildHomePage();
        }

        [Fact]
        public void VerifyNavigationToAboutEHUPage()
        {
            _homePage.NavigateToHomePage("https://en.ehu.lt/");
            _homePage.ClickAboutButton();
            Assert.Equal("About", _homePage.Driver.Title);
            var header = _homePage.GetHeader();
            Assert.True(header.Displayed, "The content header does not display the expected text.");
        }

        [Theory]
        [InlineData("study programs")]
        public void VerifySearchFunctionality(string query)
        {
            _homePage.NavigateToHomePage("https://en.ehu.lt/");
            _homePage.ClickSearchIcon();
            _homePage.EnterSearchTerm(query);
            _homePage.SubmitSearch();

            Assert.Equal("https://en.ehu.lt/?s=study+programs", _homePage.Driver.Url);

            // Assert.Contains($"/?s={query.Replace(" ", "+")}", _homePage.Driver.Url);
            // Assert.True(_homePage.SearchResultsContain("study"), "The search results do not contain the expected text.");
        }

        [Fact]
        public void VerifyLanguageChangeFunctionality()
        {
            _homePage.NavigateToHomePage("https://en.ehu.lt/");
            _homePage.ClickLanguageSwitcher();
            _homePage.SelectLithuanianLanguage();
            Assert.Equal("https://lt.ehu.lt/", _homePage.Driver.Url);
            var header = _homePage.GetLithuanianHeader();
            Assert.True(header.Displayed, "The page content does not appear in Lithuanian.");
        }

        [Fact]
        public void VerifyContactFormSubmission()
        {
            _homePage.NavigateToContactsPage();
            Assert.Equal("Admission inquiries", _homePage.GetAdmissionInquiriesText());
            Assert.Equal("recruitment@ehu.lt", _homePage.GetEmailFieldText());
            Assert.Equal("European Humanities University", _homePage.GetFacebookFieldText());
            Assert.Contains("+370 (644) 96 317", _homePage.GetPhoneFieldText());
        }

        public void Dispose()
        {
            _testBuilder.TearDown();
        }
    }
}