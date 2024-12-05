using NUnit.Framework;
using Testing_w_Selenium.TestBuilders;
using FluentAssertions;
using Serilog;
using System;

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
            Log.Debug("Test setup initialized.");
        }

        [Test]
        public void VerifyNavigationToAboutEHUPage()
        {
            var testName = nameof(VerifyNavigationToAboutEHUPage);
            Log.Information("[{TestName}] Test started.", testName);

            try
            {
                var homePage = _testBuilder.BuildHomePage();
                homePage.NavigateToHomePage("https://en.ehu.lt/");
                Log.Debug("Navigated to home page.");

                homePage.ClickAboutButton();
                Log.Debug("Clicked 'About' button.");

                homePage.Driver.Title.Should().Be("About", "The title should be 'About'.");
                Log.Debug("Verified page title.");

                var header = homePage.GetHeader();
                header.Displayed.Should().BeTrue("The content header should be displayed.");
                Log.Debug("Verified header is displayed.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[{TestName}] Test failed with exception.", testName);
                _testBuilder.BuildHomePage().CaptureScreenshot(testName);
                throw;
            }
            finally
            {
                Log.Information("[{TestName}] Test completed.", testName);
            }
        }

        [Test]
        public void VerifySearchFunctionality()
        {
            var testName = nameof(VerifySearchFunctionality);
            Log.Information("[{TestName}] Test started.", testName);

            try
            {
                var homePage = _testBuilder.BuildHomePage();
                homePage.NavigateToHomePage("https://en.ehu.lt/");
                Log.Debug("Navigated to home page.");

                homePage.ClickSearchIcon();
                Log.Debug("Clicked search icon.");

                homePage.EnterSearchTerm("study programs");
                Log.Debug("Entered search term.");

                homePage.SubmitSearch();
                Log.Debug("Submitted search.");

                homePage.Driver.Url.Should().Be("https://en.ehu.lt/?s=study+programs", "The URL should match the expected search URL.");
                Log.Debug("Verified search URL.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[{TestName}] Test failed with exception.", testName);
                _testBuilder.BuildHomePage().CaptureScreenshot(testName);
                throw;
            }
            finally
            {
                Log.Information("[{TestName}] Test completed.", testName);
            }
        }

        [Test]
        public void VerifyLanguageChangeFunctionality()
        {
            var testName = nameof(VerifyLanguageChangeFunctionality);
            Log.Information("[{TestName}] Test started.", testName);

            try
            {
                var homePage = _testBuilder.BuildHomePage();
                homePage.NavigateToHomePage("https://en.ehu.lt/");
                Log.Debug("Navigated to home page.");

                homePage.ClickLanguageSwitcher();
                Log.Debug("Clicked language switcher.");

                homePage.SelectLithuanianLanguage();
                Log.Debug("Selected Lithuanian language.");

                homePage.Driver.Url.Should().Be("https://lt.ehu.lt/", "The URL should match the expected Lithuanian version.");
                Log.Debug("Verified Lithuanian URL.");

                var header = homePage.GetLithuanianHeader();
                header.Displayed.Should().BeTrue("The page content should appear in Lithuanian.");
                Log.Debug("Verified Lithuanian header.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[{TestName}] Test failed with exception.", testName);
                _testBuilder.BuildHomePage().CaptureScreenshot(testName);
                throw;
            }
            finally
            {
                Log.Information("[{TestName}] Test completed.", testName);
            }
        }

        [Test]
        public void VerifyContactFormSubmission()
        {
            var testName = nameof(VerifyContactFormSubmission);
            Log.Information("[{TestName}] Test started.", testName);

            try
            {
                var homePage = _testBuilder.BuildHomePage();
                homePage.NavigateToContactsPage();
                Log.Debug("Navigated to contacts page.");

                homePage.GetAdmissionInquiriesText().Should().Be("Admission inquiries", "The admission inquiries text should match.");
                Log.Debug("Verified admission inquiries text.");

                homePage.GetEmailFieldText().Should().Be("recruitment@ehu.lt", "The email field text should match.");
                Log.Debug("Verified email field text.");

                homePage.GetFacebookFieldText().Should().Be("European Humanities University", "The Facebook field text should match.");
                Log.Debug("Verified Facebook field text.");

                homePage.GetPhoneFieldText().Should().Contain("+370 (644) 96 317", "The phone field text should match.");
                Log.Debug("Verified phone field text.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "[{TestName}] Test failed with exception.", testName);
                _testBuilder.BuildHomePage().CaptureScreenshot(testName);
                throw;
            }
            finally
            {
                Log.Information("[{TestName}] Test completed.", testName);
            }
        }

        [TearDown]
        public void TearDown()
        {
            Log.Debug("Test teardown initiated.");
            _testBuilder.TearDown();
            Log.Debug("Test teardown completed.");
        }
    }
}