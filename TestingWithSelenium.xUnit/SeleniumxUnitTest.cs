using System;
using Xunit;
using Testing_w_Selenium.TestBuilders;
using Testing_w_Selenium.PageObjects;
using FluentAssertions;
using Serilog;

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
            Log.Debug("Test setup initialized.");
        }

        [Fact]
        public void VerifyNavigationToAboutEHUPage()
        {
            var testName = nameof(VerifyNavigationToAboutEHUPage);
            Log.Information("[{TestName}] Test started.", testName);

            try
            {
                _homePage.NavigateToHomePage("https://en.ehu.lt/");
                Log.Debug("Navigated to home page.");

                _homePage.ClickAboutButton();
                Log.Debug("Clicked 'About' button.");

                _homePage.Driver.Title.Should().Be("About", "The title should be 'About'.");
                Log.Debug("Verified page title.");

                var header = _homePage.GetHeader();
                header.Displayed.Should().BeTrue("The content header should be displayed.");
                Log.Debug("Verified header is displayed.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[{TestName}] Test failed with exception.", testName);
                _homePage.CaptureScreenshot(testName);
                throw;
            }
            finally
            {
                Log.Information("[{TestName}] Test completed.", testName);
            }
        }

        [Theory]
        [InlineData("study programs")]
        public void VerifySearchFunctionality(string query)
        {
            var testName = nameof(VerifySearchFunctionality);
            Log.Information("[{TestName}] Test started with query '{Query}'.", testName, query);

            try
            {
                _homePage.NavigateToHomePage("https://en.ehu.lt/");
                Log.Debug("Navigated to home page.");

                _homePage.ClickSearchIcon();
                Log.Debug("Clicked search icon.");

                _homePage.EnterSearchTerm(query);
                Log.Debug("Entered search term '{Query}'.", query);

                _homePage.SubmitSearch();
                Log.Debug("Submitted search.");

                _homePage.Driver.Url.Should().Be($"https://en.ehu.lt/?s={query.Replace(" ", "+")}", "The URL should match the expected search URL.");
                Log.Debug("Verified search URL.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[{TestName}] Test failed with exception.", testName);
                _homePage.CaptureScreenshot(testName);
                throw;
            }
            finally
            {
                Log.Information("[{TestName}] Test completed.", testName);
            }
        }

        [Fact]
        public void VerifyLanguageChangeFunctionality()
        {
            var testName = nameof(VerifyLanguageChangeFunctionality);
            Log.Information("[{TestName}] Test started.", testName);

            try
            {
                _homePage.NavigateToHomePage("https://en.ehu.lt/");
                Log.Debug("Navigated to home page.");

                _homePage.ClickLanguageSwitcher();
                Log.Debug("Clicked language switcher.");

                _homePage.SelectLithuanianLanguage();
                Log.Debug("Selected Lithuanian language.");

                _homePage.Driver.Url.Should().Be("https://lt.ehu.lt/", "The URL should match the expected Lithuanian version.");
                Log.Debug("Verified Lithuanian URL.");

                var header = _homePage.GetLithuanianHeader();
                header.Displayed.Should().BeTrue("The page content should appear in Lithuanian.");
                Log.Debug("Verified Lithuanian header.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[{TestName}] Test failed with exception.", testName);
                _homePage.CaptureScreenshot(testName);
                throw;
            }
            finally
            {
                Log.Information("[{TestName}] Test completed.", testName);
            }
        }

        [Fact]
        public void VerifyContactFormSubmission()
        {
            var testName = nameof(VerifyContactFormSubmission);
            Log.Information("[{TestName}] Test started.", testName);

            try
            {
                _homePage.NavigateToContactsPage();
                Log.Debug("Navigated to contacts page.");

                _homePage.GetAdmissionInquiriesText().Should().Be("Admission inquiries", "The admission inquiries text should match.");
                Log.Debug("Verified admission inquiries text.");

                _homePage.GetEmailFieldText().Should().Be("recruitment@ehu.lt", "The email field text should match.");
                Log.Debug("Verified email field text.");

                _homePage.GetFacebookFieldText().Should().Be("European Humanities University", "The Facebook field text should match.");
                Log.Debug("Verified Facebook field text.");

                _homePage.GetPhoneFieldText().Should().Contain("+370 (644) 96 317", "The phone field text should match.");
                Log.Debug("Verified phone field text.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[{TestName}] Test failed with exception.", testName);
                _homePage.CaptureScreenshot(testName);
                throw;
            }
            finally
            {
                Log.Information("[{TestName}] Test completed.", testName);
            }
        }

        public void Dispose()
        {
            Log.Debug("Test teardown initiated.");
            _testBuilder.TearDown();
            Log.Debug("Test teardown completed.");
        }
    }
}