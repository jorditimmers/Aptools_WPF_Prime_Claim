// System c#
using System.IO;
using System.Collections.Generic;
using System.Linq;

//Selenium C#
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.DevTools;

//API requests
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;
using System.Threading;
using System;
using System.Diagnostics;
using System.Windows;

namespace modernDesign.Functions
{
    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                try
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                    return wait.Until(drv => drv.FindElement(by));
                }
                catch
                {
                    return null;
                }

            }
            try
            {
                return driver.FindElement(by);
            }
            catch
            {
                return null;
            }
        }
    }

    public class PrimeClaim
    {
        static public bool IsPresent(string elem_val, IWebDriver driver)
        {
            try
            {
                driver.FindElement(By.XPath(elem_val));
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}