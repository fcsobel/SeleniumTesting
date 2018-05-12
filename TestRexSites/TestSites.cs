using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;

namespace TestRexSites
{
	[TestClass]
	public class TestSites
	{
		[TestMethod]
		public void TestMethod1()
		{
			var driver = new ChromeDriver();

			driver.Navigate().GoToUrl("http://www.reservedirect.com/");

		}
	}
}
