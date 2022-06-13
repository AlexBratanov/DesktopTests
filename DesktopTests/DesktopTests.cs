using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
// install Appuim - NuGet Packages and run Appium server


namespace DesktopClientTests // to be added additional name
{
    public class DesktopTests
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";//to be check during the exam
        private const string ContactBookUrl = "https://contactbook.nakov.repl.co/api"; // to be changed during the exam
        private const string appLocation = @"D:\SoftUni\QA Automation\QA Automation-HomeWorks\ContactBook\ContactBook-DesktopClient.exe";
        // applocation to be changed after downloading of "zip" file and start the "exe" file instead "apk" file. 

        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;
        
        // For Mobile testing
        // start command prompt and put "adb devices" to see the name of device, which shall be added to Appium Inpector. 
        // start Appium server
        // start Android studio or connect Android device to the laptop
        // start Appium Inspector - here you shall put "platformName", appium: app - the path to "apk" file, and deviceName.


        [SetUp]
        public void StartApp()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]

        public void CloseApp()
        {
            driver.Quit();
        }
        [Test]
        public void Test_SearchContact_VerifyFirstResult()
        {
            // Arrange
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(ContactBookUrl);

            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click();

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            var editTextField = driver.FindElementByAccessibilityId("textBoxSearch");
            editTextField.SendKeys("steve");

            //Act
            var buttonSearch = driver.FindElementByAccessibilityId("buttonSearch");
            buttonSearch.Click();

            //Case1:
            // Thread.Sleep(2000);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            var element = wait.Until(d =>
            {
                var searchLabel = driver.FindElementByAccessibilityId("labelResult").Text;
                return searchLabel.StartsWith("Contacts found");
            });

            var searchLabel = driver.FindElementByAccessibilityId("labelResult").Text;
            Assert.That(searchLabel, Is.EqualTo("Contacts found: 1"));

            // Assert
            var firstName = driver.FindElement(By.XPath("//Edit[@Name=\"FirstName Row 0, Not sorted.\"]"));
            var lastName = driver.FindElement(By.XPath("//Edit[@Name=\"LastName Row 0, Not sorted.\"]"));

            Assert.That(firstName.Text, Is.EqualTo("Steve"));
            Assert.That(lastName.Text, Is.EqualTo("Jobs"));
        }
    }
}