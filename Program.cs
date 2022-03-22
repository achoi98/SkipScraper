using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;
using System.Reflection;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace HTMLParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = SkipTheDishes.GetUrl();
            string path = SkipTheDishes.GetPath();
            IWebDriver driver = SkipTheDishes.InitializeDriver(url);

            string storeNameXPath = "//*[@id='root']/div/main/div/div/div/div[1]/div/div[2]/div/div/div/div[1]/h1";

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            try
            {
                var storeName = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(storeNameXPath)));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Unable to find store. Please confirm the URL and try again.\nExiting...");
                Environment.Exit(0);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Unable to find store. Please confirm the URL and try again.\nExiting...");
                Environment.Exit(0);
            }

            var name = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(storeNameXPath)));
            Console.WriteLine("Store Name: " + name.Text + "\n");
            System.DateTime dateTime = DateTime.Now;
            path += "/skip_" + name.Text + "_" + dateTime.ToString("yyyy-MM-dd") + ".txt";
            List<string> lines = new List<string>();
            lines.Add("CATEGORY,NAME,PRICE");

            var listingsPath = "/html/body/div[2]/div/main/div/div/div/div[2]/div/div/div/div[1]/div[2]/div";
            var listings = driver.FindElement(By.XPath(listingsPath));
            var categories = listings.FindElements(By.XPath("./*"));



            Console.WriteLine("Number of categories: " + categories.Count());
            for (int i = 3; i <= categories.Count(); i++)
            {
                var categoryPath = "/html/body/div[2]/div/main/div/div/div/div[2]/div/div/div/div[1]/div[2]/div/div[" + i + "]";
                var categoryHeaderPath = categoryPath + "/div/div/div/h2";
                var categoryItemsListPath = categoryPath + "/div/div/ul";
                
                string categoryName = driver.FindElement(By.XPath(categoryHeaderPath)).Text;
                
                // ignore non-menu categories
                if (categoryName.Contains("Age Verification") ||
                    categoryName.Contains("Contactless Recommendations") || 
                    categoryName.Contains("Community Support"))
                {
                    continue;
                }
                var itemsList = listings.FindElement(By.XPath(categoryItemsListPath));
                IList<IWebElement> items = itemsList.FindElements(By.TagName("li"));
                Console.WriteLine("Items count: " + items.Count());
                foreach (IWebElement item in items)
                {
                    string s = "";
                    s += "\"" + categoryName + "\"";
                    SkipMenuItemModel itemModel = SkipTheDishes.CreateMenuItem(item);
                    s += ",\"" + itemModel.Name + "\"";
                    s += "," + itemModel.Price;
                    
                    lines.Add(s);
                    Console.WriteLine(s);
                }
                Console.WriteLine("----------------------------------------");
            }
            Console.WriteLine("\nTotal number of items added: " + lines.Count);
            File.WriteAllLines(path, lines);
            driver.Quit();


        }


    }
}