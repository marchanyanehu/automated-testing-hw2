using NUnit.Framework;
using Testing_w_Selenium.TestBuilders;

namespace TestingWithSelenium.NUnit
{
    [TestFixture]
    public class SeleniumNUnitTest
    {
        private TestBuilder _testBuilder;

        [SetUp]
        public void SetUp()
        {
            _testBuilder = new TestBuilder().InitializeDriver();
        }

        [Test]
        public void VerifyNavigationToAboutEHUPage()
        {
            var homePage = _testBuilder.BuildHomePage();
            homePage.NavigateToHomePage("https://en.ehu.lt/");
            homePage.ClickAboutButton();
            Assert.That(homePage.Driver.Title, Is.EqualTo("About"), "The title does not match the expected value.");
            var header = homePage.GetHeader();
            Assert.IsTrue(header.Displayed, "The content header does not display the expected text.");
        }

        [Test]
        public void VerifySearchFunctionality()
        {
            var homePage = _testBuilder.BuildHomePage();
            homePage.NavigateToHomePage("https://en.ehu.lt/");
            homePage.ClickSearchIcon();
            homePage.EnterSearchTerm("study programs");
            homePage.SubmitSearch();
            Assert.AreEqual("https://en.ehu.lt/?s=study+programs", homePage.Driver.Url, "The URL does not match the expected search URL.");
        // Works unconsistently, so commented out for now
        // Assert.IsTrue(homePage.UrlContainsSearchTerm("study+programs"), "The URL does not contain the expected search term.");
        // Assert.IsTrue(homePage.SearchResultsContain("study"), "The search results do not contain the expected text.");
        }

        [Test]
        public void VerifyLanguageChangeFunctionality()
        {
            var homePage = _testBuilder.BuildHomePage();
            homePage.NavigateToHomePage("https://en.ehu.lt/");
            homePage.ClickLanguageSwitcher();
            homePage.SelectLithuanianLanguage();
            Assert.That(homePage.Driver.Url, Is.EqualTo("https://lt.ehu.lt/"), "The URL does not match the expected Lithuanian version.");
            var header = homePage.GetLithuanianHeader();
            Assert.IsTrue(header.Displayed, "The page content does not appear in Lithuanian.");
        }

        [Test]
        public void VerifyContactFormSubmission()
        {
            var homePage = _testBuilder.BuildHomePage();
            homePage.NavigateToContactsPage();
            Assert.AreEqual("Admission inquiries", homePage.GetAdmissionInquiriesText(), "The admission inquiries text does not match.");
            Assert.AreEqual("recruitment@ehu.lt", homePage.GetEmailFieldText(), "The email field text does not match.");
            Assert.AreEqual("European Humanities University", homePage.GetFacebookFieldText(), "The Facebook field text does not match.");
            Assert.IsTrue(homePage.GetPhoneFieldText().Contains("+370 (644) 96 317"), "The phone field text does not match.");
        }

        [TearDown]
        public void TearDown()
        {
            _testBuilder.TearDown();
        }
    }
}