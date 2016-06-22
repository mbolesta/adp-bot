using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using By = OpenQA.Selenium.Extensions.By;

namespace adp_bot
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			using (var driver = new PhantomJSDriver())
			{
				SignInToAdpWebsite(driver);
				ApproveAllPendingPtoRequests(driver);
			}
		}

		private static void ApproveAllPendingPtoRequests(IWebDriver driver)
		{
			driver.Navigate().GoToUrl("https://workforcenow.adp.com/portal/theme#/MyTeam_ttd_MyTeamTabPTOCategoryListOfRequests/MyTeamTabPTOCategoryListOfRequests");

			var acceptAllRequestsLink = Wait(driver).Until(ExpectedConditions.ElementToBeClickable(By.JQuerySelector("#markAllApprovedLink")));
			acceptAllRequestsLink.Click();
		}

		private static WebDriverWait Wait(IWebDriver driver)
		{
			return new WebDriverWait(driver, TimeSpan.FromSeconds(5));
		}

		private static void SignInToAdpWebsite(IWebDriver driver)
		{
			driver.Navigate().GoToUrl("https://workforcenow.adp.com/public/index.htm");

			var loginForm = driver.FindElement(By.JQuerySelector(".login-form"));
			var username = loginForm.FindElement(By.JQuerySelector("input[name='USER']"));
			username.Clear();
			username.SendKeys(ConfigurationManager.AppSettings["adp.username"]);

			var password = loginForm.FindElement(By.JQuerySelector("input[name='PASSWORD']"));
			password.Clear();
			password.SendKeys(ConfigurationManager.AppSettings["adp.password"]);

			var submitButton = loginForm.FindElement(By.Id("portal.login.logIn"));
			submitButton.Click();
		}
	}
}