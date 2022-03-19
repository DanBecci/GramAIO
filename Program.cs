<<<<<<< HEAD
﻿
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading.Tasks;
=======
// (Source code by slash#1995 on Discord)

using System;
>>>>>>> 0f2b1409f00397a41f2e8c082878d220d71df386
using System.Windows.Forms;

namespace GramAIO
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string currentUpdate = "1.0.0.5";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // normally I would setup a SQL server to indicate an update change and use the Dropbox Api to get the update,
            // but since this source code is not private, I will not be sharing any login tokens, rather I will be making the auto-update
            // system by using pastebin and dropbox website. pastebin will indicate an update change and a driver will be used to fetch the file on dropbox                 
                try
                {
                    var url = "https://pastebin.com/JWHHxvwr";
                    var web = new HtmlWeb();
                    var doc = web.Load(url);
                    if (doc.DocumentNode.InnerHtml.ToString().Contains(currentUpdate) && doc.DocumentNode.InnerHtml.ToString().Contains("GramAIO 2022"))
                    {
                        Application.Run(new Form1());
                    }
                    else
                    {
                        if (doc.DocumentNode.InnerHtml.ToString().Contains("GramAIO 2022"))
                        {
                            ChromeOptions optionsRenew = new ChromeOptions();
                            optionsRenew.AddArgument("--incognito");
                            optionsRenew.AddArgument("--silent");

                            var chromeDriverServiceRenew = ChromeDriverService.CreateDefaultService();
                            chromeDriverServiceRenew.HideCommandPromptWindow = true;
                            var driverRenew = new ChromeDriver(chromeDriverServiceRenew, optionsRenew);
                            MessageBox.Show("Update available!");
                            driverRenew.Navigate().GoToUrl("https://www.dropbox.com/s/xz5bqc8c5zr8g3t/GramAIO%20Setup.exe?dl=1");
                        }      
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error checking for update.");
                }
            
        }
    }
}
