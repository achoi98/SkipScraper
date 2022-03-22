using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser
{
    internal class SkipTheDishes
    {
        static string[] volumesUnderOneL =
        {
            "50ml", "50 ml", "200ml", "200 ml", "355ml", "355 ml",
            "375ml", "375 ml",
            "473ml", "473 ml", "500ml", "500 ml", "710ml", "710 ml",
            "740ml", "750 ml", "750ml", "750 ml"
        };
        static string[] volumesOverOneL =
        {
            "1l", "1 l", "1.14l", "1.14 l", "1.5l", "1.5 l", "1.75l", "1.75 l",
            "1.89l", "1.89 l", "2l", "2 l", "3l", "3 l", "4l", "4 l"
        };
        static string[] containersFour =
        {
            "4cans", "4 cans", "4-cans",
            "4bottles", "4 bottles", "4-bottles",
            "4pk", "4 pk", "4-pk", "4pack", "4 pack", "4-pack",
        };
        static string[] containersSix =
        {
            "6cans", "6 cans", "6-cans",
            "6bottles", "6 bottles", "6-bottles",
            "6pk", "6 pk", "6-pk", "6pack", "6 pack", "6-pack",
        };
        static string[] containersEight =
        {
            "8cans", "8 cans", "8-cans",
            "8bottles", "8 bottles", "8-bottles",
            "8pk", "8 pk", "8-pk", "8pack", "8 pack", "8-pack",
        };
        static string[] containersTwelve =
        {
            "12cans", "12 cans", "12-cans",
            "12bottles", "12 bottles", "12-bottles",
            "12pk", "12 pk", "12-pk", "12pack", "12 pack", "12-pack",
        };
        static string[] containersFifteen =
        {
            "15cans", "15 cans", "15-cans",
            "15bottles", "15 bottles", "15-bottles",
            "15pk", "15 pk", "15-pk", "15pack", "15 pack", "15-pack",
        };
        static string[] containersTwentyFour =
        {
            "24cans", "24 cans", "24-cans",
            "24bottles", "24 bottles", "24-bottles",
            "24pk", "24 pk", "24-pk", "24pack", "24 pack", "24-pack",
        };
        static string[] containersFourtyEight =
        {
            "48cans", "48 cans", "48-cans",
            "48bottles", "48 bottles", "48-bottles",
            "48pk", "48 pk", "48-pk", "48pack", "48 pack", "48-pack",
        };

        public static IWebDriver InitializeDriver(string url)
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.EnableVerboseLogging = false;
            service.SuppressInitialDiagnosticInformation = true;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--disable-logging");
            options.AddArgument("--log-level=3");
            IWebDriver driver = new ChromeDriver(service, options);
            driver.Url = url;
            driver.Manage().Window.Minimize();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(driver.Title);
            return driver;
        }


        public static string GetUrl()
        {
            Console.WriteLine("Enter SkipTheDishes Restaurant Page URL (ABSOLUTE): ");
            string? url = Console.ReadLine();

            while (url == null || !url.StartsWith("https://www.skipthedishes.com/"))
            {
                Console.WriteLine("Invalid URL. The url should begin with 'https://www.skipthedishes.com/'");
                url = Console.ReadLine();
            }

            return url;
        }

        public static string GetPath()
        {
            string? path = "";
            Console.WriteLine("Enter the path to the folder to save to: ");
            path = Console.ReadLine();
            while (path == null || path.Length == 0 || !Directory.Exists(path))
            {
                Console.WriteLine("Invalid path. Enter a valid path to a directory: ");
                path = Console.ReadLine();
            }
            return path;
        }

        public static SkipMenuItemModel CreateMenuItem(IWebElement item)
        {
            SkipMenuItemModel itemModel = new SkipMenuItemModel();
            string itemName = item.FindElement(By.CssSelector("span[itemprop='name']")).Text;
            string itemPrice = item.FindElement(By.CssSelector("meta[itemprop='price']")).FindElement(By.XPath("./following-sibling::h4")).Text;

            itemModel.Name = itemName;
            itemModel.Price = itemPrice;


            return itemModel;
        }

        // todo
        public static string GetVolumeFromName(string name)
        {
            string s = "";

            return s;
        }

      
        // will implement regex system after I learn it
        public static string GetDepositFromName(string name)
        {
            // TODO - implement wildcard matching with regex to clean up everything

            name = name.ToLower();

            // the following comparisons determine if the name contains some form of 'x-pack'
            // deposit is determined as x * 0.10, assuming the individual units are <= 1L in volume
            if (containersFour.Any(name.Contains))
            {
                return "$0.40";
            }
            if (containersSix.Any(name.Contains))
            {
                return "$0.60";
            }
            if (containersEight.Any(name.Contains))
            {
                return "$0.80";
            }
            if (containersTwelve.Any(name.Contains))
            {
                return "$1.20";
            }
            if (containersFifteen.Any(name.Contains))
            {
                return "$1.50";
            }
            if (containersTwentyFour.Any(name.Contains))
            {
                return "$2.40";
            }
            if (containersFourtyEight.Any(name.Contains))
            {
                return "$4.80";
            }

            if (volumesUnderOneL.Any(name.Contains))
            {
                return ("$0.10");
            }
            if (volumesOverOneL.Any(name.Contains))
            {
                return ("0.25");
            }

            return string.Empty;
        }
    }
}
