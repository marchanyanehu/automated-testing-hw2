using TechTalk.SpecFlow;
using Testing_w_Selenium.TestBuilders;
using Testing_w_Selenium.PageObjects;
using FluentAssertions;
using Serilog;

namespace Testing_w_Selenium.StepDefinitions
{
    [Binding]
    public class EHUWebsiteSteps
    {
        private TestBuilder _testBuilder;
        private HomePage _homePage;

        [BeforeScenario]
        public void SetUp()
        {
            _testBuilder = new TestBuilder().InitializeDriver();
            _homePage = _testBuilder.BuildHomePage();
            Log.Debug("Test setup initialized in Step Definitions.");
        }

        [Given(@"I am on the EHU home page")]
        public void GivenIAmOnTheEHUHomePage()
        {
            _homePage.NavigateToHomePage("https://en.ehu.lt/");
            Log.Debug("Navigated to EHU home page.");
        }

        [When(@"I navigate to the About page")]
        public void WhenINavigateToTheAboutPage()
        {
            _homePage.ClickAboutButton();
            Log.Debug("Clicked 'About' button.");
        }

        [Then(@"I should see the About page header")]
        public void ThenIShouldSeeTheAboutPageHeader()
        {
            var header = _homePage.GetHeader();
            header.Displayed.Should().BeTrue("The About page header should be displayed.");
            Log.Debug("Verified About page header is displayed.");
        }

        [When(@"I search for ""(.*)""")]
        public void WhenISearchFor(string searchTerm)
        {
            _homePage.ClickSearchIcon();
            _homePage.EnterSearchTerm(searchTerm);
            _homePage.SubmitSearch();
            Log.Debug($"Searched for '{searchTerm}'.");
        }

        [Then(@"the search results should contain ""(.*)""")]
        public void ThenTheSearchResultsShouldContain(string searchTerm)
        {
            var resultsContainKeyword = _homePage.SearchResultsContain(searchTerm);
            resultsContainKeyword.Should().BeTrue($"Search results should contain '{searchTerm}'.");
            Log.Debug($"Verified search results contain '{searchTerm}'.");
        }

        [When(@"I change the language to Lithuanian")]
        public void WhenIChangeTheLanguageToLithuanian()
        {
            _homePage.ClickLanguageSwitcher();
            _homePage.SelectLithuanianLanguage();
            Log.Debug("Changed language to Lithuanian.");
        }

        [Then(@"the page should be displayed in Lithuanian")]
        public void ThenThePageShouldBeDisplayedInLithuanian()
        {
            var header = _homePage.GetLithuanianHeader();
            header.Displayed.Should().BeTrue("The page should be displayed in Lithuanian.");
            Log.Debug("Verified that the page is displayed in Lithuanian.");
        }

        [Given(@"I am on the Contacts page")]
        public void GivenIAmOnTheContactsPage()
        {
            _homePage.NavigateToContactsPage();
            Log.Debug("Navigated to Contacts page.");
        }

        [When(@"I view the Admission Inquiries section")]
        public void WhenIViewTheAdmissionInquiriesSection()
        {
            Log.Debug("Viewing the Admission Inquiries section.");
        }

        [Then(@"I should see the correct contact information")]
        public void ThenIShouldSeeTheCorrectContactInformation()
        {
            var admissionInquiriesText = _homePage.GetAdmissionInquiriesText();
            admissionInquiriesText.Should().NotBeNullOrEmpty("Admission Inquiries text should be present.");
            Log.Debug("Verified Admission Inquiries text is present.");

            var emailText = _homePage.GetEmailFieldText();
            emailText.Should().Contain("@ehu.lt", "Email should contain '@ehu.lt'.");
            Log.Debug("Verified email field contains '@ehu.lt'.");

            var phoneText = _homePage.GetPhoneFieldText();
            phoneText.Should().MatchRegex(@"\+?\d+", "Phone number should contain digits.");
            Log.Debug("Verified phone field contains digits.");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Log.Debug("Test teardown initiated in Step Definitions.");
            _testBuilder.TearDown();
            Log.Debug("Test teardown completed in Step Definitions.");
        }
    }
}