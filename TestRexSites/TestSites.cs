using System;
using System.Collections.Generic;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NHtmlUnit;
using NHtmlUnit.Html;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;


namespace TestRexSites
{
	[TestFixture]
	public class TestSites
	{
		private WebClient webClient;

		public ExtentReports extent;
		public ExtentTest test;
		public ExtentTest childTest;
		public ExtentHtmlReporter htmlReporter;

		[OneTimeSetUp]
		public void TestSetup()
		{
			webClient = new WebClient();

			var path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
			string actuaPath = path.Substring(0, path.IndexOf("bin"));
			string projectPath = new Uri(actuaPath).LocalPath;

			string reportPath = projectPath + "Reports\\MyOwnReport.html";

			// initialize ExtentReports and attach the HtmlReporter
			extent = new ExtentReports();

			// initialize the HtmlReporter
			htmlReporter = new ExtentHtmlReporter(reportPath);

			// load config
			htmlReporter.LoadConfig(projectPath + "extent-config.xml");

			// attach only HtmlReporter
			extent.AttachReporter(htmlReporter);

			extent.AddSystemInfo("Host Name", "Test");
			extent.AddSystemInfo("Environment", "QA");
			extent.AddSystemInfo("User Name", "Joe");

			
		}

		[Test]
		public void Can_Load_Google_Homepage()
		{
			var resp = webClient.GetPage("http://www.google.com").WebResponse;
			Assert.IsTrue(resp.StatusCode == 200);
		}

		[Test]
		public void Can_Load_Rd_Sites()
		{
			this.test = extent.CreateTest("Test Sites");
			this.TestSite(this.test, "http://www.reservenewyorkcity.com", "new-york");
			this.TestSite(this.test, "http://www.reservebranson.com", "branson");
		}

		public void TestSite(ExtentTest parent, string url, string dest)
		{
			List<string> sites = new List<string>()
			{
				"/",
				"/{0}-shows",
				"/{0}-attractions",
				"/{0}-hotels",
				"/{0}-packages",
			};

			webClient.Options.ThrowExceptionOnScriptError = false;
			webClient.Options.ThrowExceptionOnFailingStatusCode = false;
			//webClient.Options. = false;
			//			webClient.ThrowExceptionOnScriptError = false;

			//this.test = extent.CreateTest(url);
			//parent = parent.CreateNode(url);

			foreach (var item in sites)
			{
				//extent.CreateTest().CreateNode
				///var childTest = parent.CreateNode(item);

				var path = string.Format("{0}{1}", url, string.Format(item, dest));

				var childTest = parent.CreateNode(path);

				var resp = webClient.GetPage(path).WebResponse;

				var checkStatus = resp.StatusCode == 200;

				if (checkStatus)
				{
					childTest.Log(Status.Pass, path);
				}
				else
				{
					childTest.Log(Status.Fail, path);
				}

				

				//Assert.IsTrue(resp.StatusCode == 200);
			}
		}


		[Test]
		public void TestMethod1()
		{
			var url = "http://www.reservenewyorkcity.com";

			List<string> sites = new List<string>()
			{
				"/",
				"/new-york-shows",
				"/euifghre",
				"/new-york-attractions",
				"/new-york-hotels",
				"/new-york-packages",
				"/new-york-packages",
				"/euifghre",
			};


			var driver = new ChromeDriver();

			foreach (var item in sites)
			{
				this.test = extent.CreateTest(item);

				var path = string.Format("{0}{1}", url, item);

				driver.Navigate().GoToUrl(path);

				if (driver.FindElementByTagName("body") != null)
				{
					this.test.Log(Status.Pass, "found body");
				}
				else
				{
					this.test.Log(Status.Fail, "unable to find body");
				}

				Assert.IsTrue(driver.FindElementByTagName("body") != null);

				//Assert.IsTrue(true);

				//this.test.Log(Status.Pass, "Assert Pass condition is true");
			}

			driver.Quit();
		}

		[Test]
		public void TestMethod2()
		{
			test = extent.CreateTest("DemoFail");

			Assert.IsTrue(false);

			test.Log(Status.Fail, "Assert Pass condition is false");
		}

		[TearDown]
		public void GetResult()
		{
			var statis = TestContext.CurrentContext.Result.Outcome.Status;
			var strackTrave = string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.StackTrace); 
			var errormessage = TestContext.CurrentContext.Result.Message;

			if (statis == NUnit.Framework.Interfaces.TestStatus.Failed)
			{
				test.Log(Status.Fail, strackTrave + errormessage);
			}
			
			//extent.Flush();

			//htmlReporter.Stop();
		}


		[OneTimeTearDown]
		public void TearOdwn()
		{
			//htmlReporter.Start();
			//htmlReporter.Stop();
			extent.Flush();
			webClient.Close();
		}	
	}
}
