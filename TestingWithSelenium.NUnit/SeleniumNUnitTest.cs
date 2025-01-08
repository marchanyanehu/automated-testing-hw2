using NUnit.Framework;
using Testing_w_Selenium.TestBuilders;
using FluentAssertions;
using Serilog;
using System;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;

namespace TestingWithSelenium.NUnit
{
    [TestFixture]
    public class SeleniumNUnitTest
    {
        private TestBuilder _testBuilder;
        private static ExtentReports _extent;
        private ExtentTest _test;

        [OneTimeSetUp]
        public void InitializeReport()
        {
            var reportPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "TestReport.html");
            _extent = new ExtentReports();
            var htmlReporter = new ExtentHtmlReporter(reportPath);
            
            htmlReporter.Config.DocumentTitle = "Test Automation Report";
            htmlReporter.Config.ReportName = "Selenium Test Results";
            
            _extent.AttachReporter(htmlReporter);
            _extent.AddSystemInfo("Environment", "Test");
            _extent.AddSystemInfo("User", Environment.UserName);
        }

        [SetUp]
        public void SetUp()
        {
            _testBuilder = new TestBuilder().InitializeDriver();
            _test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
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

                _test.Pass("Navigation to About page successful");
            }
            catch (Exception ex)
            {
                _test.Fail(ex);
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

                homePage.Driver.Url.Should().Be("https://en.ehu.lt/wrong-url", "The URL should match the expected search URL.");
                
                _test.Pass("Search functionality verified");
            }
            catch (Exception ex)
            {
                _test.Fail(ex);
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
        [Ignore("Test skipped for demonstration purposes")]
        public void VerifyLanguageChangeFunctionality()
        {
            _test.Skip("Test skipped for demonstration purposes");
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
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var errorMessage = TestContext.CurrentContext.Result.Message;
            
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                _test.Fail(errorMessage);
            }
            else if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Skipped)
            {
                _test.Skip(errorMessage);
            }
            else
            {
                _test.Pass("Test executed successfully");
            }

            Log.Debug("Test teardown initiated.");
            _testBuilder.TearDown();
            Log.Debug("Test teardown completed.");
        }

        [OneTimeTearDown]
        public void CleanupReport()
        {
            _extent.Flush();
        }
    }
}