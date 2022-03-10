using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GramAIO
{
    public partial class GramAIO : Form
    {
        private bool loggedIn = false;
        private int numCounter = 0;
        int placeCounter;
        int listElemCounter;
        private bool paused = false;
        public GramAIO()
        {
            InitializeComponent();
            pswTB.PasswordChar = '*';
            richTextBox1.Text = "Enter tags, one per line. Do not include '#' in your tags!";
            richTextBox2.Text = "Enter users, one per line. Do not include '@' in your users!";
            richTextBox3.Text = "Enter comment to post. Only one comment allowed!";
            usrTB.ForeColor = Color.Red;
            pswTB.ForeColor = Color.Red;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(label6.ForeColor == Color.Red || label5.ForeColor == Color.Red || label4.ForeColor == Color.Red)
            {
                MessageBox.Show("Please fix the errors in red!");
                return;
            }
            if (checkBox4.Checked && checkBox1.Checked)
            {
                MessageBox.Show("You cannot run the follow and the unfollow module at the same time! Deselect one of the two modules and try again.");
                return;
            }
            try
            {
                if (Convert.ToInt32(textBox1.Text) >= 900000)
                {
                    MessageBox.Show("Your delay time must be less than 15 minutes!");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Your delay time must be less than 15 minutes!");
                return;
            }

            try
            {
                if (Convert.ToInt32(textBox2.Text) >= 30)
                {
                    MessageBox.Show("Your interaction count must be less than 30");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Your interaction count must be less than 30");
                return;
            }


            List<string> list = new List<string>();
            foreach (string line in richTextBox1.Lines)
            {
                list.Add(line);
            }
            if (loggedIn == false)
            {
                new Task(() =>
                {
                    ChromeOptions optionsRenew = new ChromeOptions();
                    optionsRenew.AddArgument("--incognito");
                    var chromeDriverServiceRenew = ChromeDriverService.CreateDefaultService();
                    chromeDriverServiceRenew.HideCommandPromptWindow = true;
                    var driverRenew = new ChromeDriver(chromeDriverServiceRenew, optionsRenew);

                    try
                    {
                        // logging the user in.
                        driverRenew.Navigate().GoToUrl("https://www.instagram.com/accounts/login/");
                        WebDriverWait w = new WebDriverWait(driverRenew, TimeSpan.FromSeconds(20));
                        w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Name("username")));
                        var findAmount = driverRenew.FindElement(By.Name("username"));
                        driverRenew.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                        findAmount.Click();
                        findAmount.SendKeys(usrTB.Text);
                        driverRenew.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                        w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Name("password")));
                        var findAmount2 = driverRenew.FindElement(By.Name("password"));
                        driverRenew.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                        findAmount2.Click();
                        findAmount2.SendKeys(pswTB.Text);
                        driverRenew.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                        w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("#loginForm > div > div:nth-child(3) > button > div")));
                        var findAmount3 = driverRenew.FindElement(By.CssSelector("#loginForm > div > div:nth-child(3) > button > div"));
                        driverRenew.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                        findAmount3.Click();
                        w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("#react-root > section > main > div > div > div > div > button")));
                        driverRenew.Navigate().GoToUrl("https://www.instagram.com/" + usrTB.Text + "/");
                        loggedIn = true;

                        // Logic part of program to handle module to run, all other code should be handled in their own classes

                        if (checkBox1.Checked)
                        {
                            // Follow module

                            //TAG
                            numCounter = 0;
                            //POST
                            listElemCounter = 0;
                            //INTERACTIONS
                            placeCounter = 0;
                            List<string> listElem2 = new List<string>();
                            async Task FollowModule()
                            {
                                if (checkBox5.Checked)
                                {
                                    if(listElemCounter < 9)
                                    {
                                        listElemCounter = 9;
                                    }                            
                                }
                                else
                                {
                                    listElemCounter = 0;
                                }
                                try
                                {
                                    while (paused == true)
                                    {

                                    }
                                    List<string> listElem = new List<string>();
                                    Thread.Sleep(Convert.ToInt32(textBox1.Text));
                                    driverRenew.Navigate().GoToUrl("https://www.instagram.com/explore/tags/" + list[numCounter] + "/");
                                    var elem = driverRenew.FindElements(By.XPath("//a[@href]"));
                                    foreach (var elems in elem)
                                    {
                                        string ELEM = elems.GetAttribute("href");
                                        listElem.Add(ELEM);
                                    }
                                    Thread.Sleep(Convert.ToInt32(textBox1.Text));
                                    if(listElem[listElemCounter] == null || listElem[listElemCounter] == "https://www.instagram.com/")
                                    {
                                        numCounter++;
                                        placeCounter = 0;
                                        FollowModule();
                                    }
                                    if (listElem2.Contains(listElem[listElemCounter]))
                                    {
                                        listElemCounter++;
                                    }
                                    driverRenew.Navigate().GoToUrl(listElem[listElemCounter]);
                                    listElem2.Add(listElem[listElemCounter]);
                                    w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("button.sqdOP:nth-child(2) > div:nth-child(1)")));
                                    Thread.Sleep(Convert.ToInt32(textBox1.Text));
                                    driverRenew.FindElement(By.CssSelector("button.sqdOP:nth-child(2) > div:nth-child(1)")).Click();
                                    Thread.Sleep(Convert.ToInt32(textBox1.Text));
                                    driverRenew.Navigate().Refresh();
                                    if (checkBox2.Checked)
                                    {
                                        //like the post
                                        //w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("/html/body/div[1]/section/main/div/div[1]/article/div/div[2]/div/div[2]/section[1]/span[1]/button/div[1]/svg")));
                                        Thread.Sleep(Convert.ToInt32(textBox1.Text));
                                        var svgObject = driverRenew.FindElement(By.CssSelector("button span > svg[aria-label='Like']"));
                                        Actions builder = new Actions(driverRenew);
                                        builder.MoveToElement(svgObject).Click().Build().Perform();
                                    }

                                    if (checkBox3.Checked)
                                    {
                                        //comment on post

                                        Thread.Sleep(Convert.ToInt32(textBox1.Text));
                                        var svgObject = driverRenew.FindElement(By.XPath("/html/body/div[1]/section/main/div/div[1]/article/div/div[2]/div/div[2]/section[3]/div/form/textarea"));
                                        Actions builder = new Actions(driverRenew);
                                        builder.MoveToElement(svgObject).Click().Build().Perform();
                                        /*
                                        Thread.Sleep(Convert.ToInt32(textBox1.Text));
                                        svgObject.SendKeys(richTextBox3.Text);
                                        Thread.Sleep(Convert.ToInt32(textBox1.Text));
                                        svgObject = driverRenew.FindElement(By.XPath("/html/body/div[1]/section/main/div/div[1]/article/div/div[2]/div/div[2]/section[3]/div/form/button/div"));
                                        builder.MoveToElement(svgObject).Click().Build().Perform();
                                        */

                                        var wb = driverRenew.FindElement(By.XPath("/html/body/div[1]/section/main/div/div[1]/article/div/div[2]/div/div[2]/section[3]/div/form/textarea"));
                                        wb.SendKeys(richTextBox3.Text);
                                        // IJavaScriptExecutor jse = (IJavaScriptExecutor)driverRenew;
                                        // jse.ExecuteScript($"arguments[0].value='{richTextBox3.Text}';", wb);

                                        Thread.Sleep(Convert.ToInt32(textBox1.Text));
                                        var svgObject2 = driverRenew.FindElement(By.XPath("/html/body/div[1]/section/main/div/div[1]/article/div/div[2]/div/div[2]/section[3]/div/form/button/div"));
                                        Actions builder2 = new Actions(driverRenew);
                                        builder2.MoveToElement(svgObject2).Click().Build().Perform();
                                    }

                                    listElemCounter++;
                                    placeCounter++;
                                    if (placeCounter == Convert.ToInt32(textBox2.Text))
                                    {
                                        numCounter++;
                                        placeCounter = 0;
                                        listElemCounter = 0;
                                    }
                                    FollowModule();
                                }
                                catch (Exception)
                                {
                                    //TODO: renable this below
                                    //driverRenew.Quit();
                                    //loggedIn = false;
                                }
                            }
                            FollowModule();
                        }
                        else if (checkBox2.Checked)
                        {
                            // Like module
                        }
                        else if (checkBox3.Checked)
                        {
                            // Comment module
                        }
                        else if (checkBox1.Checked && checkBox2.Checked && checkBox3.Checked)
                        {
                            // AIO module
                        }

                    }
                    // error occured while logging in, dispose of the browser and prompt the user of the occurence.
                    // possibly want to change this to a retry in 30 minutes handler
                    catch (Exception)
                    {
                        //TODO: make this valid below
                        //driverRenew.Quit();
                        //loggedIn = false;
                        //MessageBox.Show("There was an error!");
                    }
                }).Start();
            }
            else
            {
                MessageBox.Show("Sorry, only one login at a time!");
            }
        }

        private void usrTB_Click(object sender, EventArgs e)
        {
            usrTB.Text = "";
        }

        private void pswTB_Click(object sender, EventArgs e)
        {
            pswTB.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            numCounter = 0;
            loggedIn = false;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C taskkill.exe /IM chromedriver.exe /F";
            process.StartInfo = startInfo;
            process.Start();

            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C taskkill.exe /IM chrome.exe /F";
            process.StartInfo = startInfo;
            process.Start();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void usrTB_Leave(object sender, EventArgs e)
        {
            if (usrTB.Text == "")
            {
                usrTB.Text = "USERNAME";
            }
        }

        private void pswTB_Leave(object sender, EventArgs e)
        {
            if (pswTB.Text == "")
            {
                pswTB.Text = "PASSWORD";
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "DELAY TIME";
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "INTERACTION #";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            numCounter++;
            placeCounter = 0;
            listElemCounter = 0;
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
                richTextBox1.Text = "Enter tags, one per line. Do not include '#' in your tags!";
            }
        }

        private void richTextBox2_Leave(object sender, EventArgs e)
        {
            if (richTextBox2.Text == "")
            {
                richTextBox2.Text = "Enter users, one per line. Do not include '@' in your users!";
            }
        }

        private void richTextBox3_Leave(object sender, EventArgs e)
        {
            if (richTextBox3.Text == "")
            {
                richTextBox3.Text = "Enter comment to post. Only one comment allowed!";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (paused == true)
            {
                paused = false;
                button4.Text = "PAUSE";
            }
            else
            {
                paused = true;
                button4.Text = "START";
            }
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "Enter tags, one per line. Do not include '#' in your tags!")
            {
                richTextBox1.Text = "";
            }
        }

        private void richTextBox2_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text == "Enter users, one per line. Do not include '@' in your users!")
            {
                richTextBox2.Text = "";
            }
        }

        private void richTextBox3_Click(object sender, EventArgs e)
        {
            if (richTextBox3.Text == "Enter comment to post. Only one comment allowed!")
            {
                richTextBox3.Text = "";
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            int chars = richTextBox3.Text.Length;
            label4.Text = $"C: ({chars}/2200)";
            if(chars > 2200)
            {
                label4.ForeColor = Color.Red;
            }
            else
            {
                label4.ForeColor = Color.FromArgb(251, 173, 80);
            }

            char ch = '#';
            int freq = 0;
            foreach (char c in richTextBox3.Text)
            {
                if (c == ch)
                {
                    freq++;
                }
            }
            label5.Text = $"#: ({freq}/30)";
            if(freq > 30)
            {
                label5.ForeColor = Color.Red;
            }
            else
            {
                label5.ForeColor = Color.FromArgb(251, 173, 80);
            }




            char ch2 = '@';
            int freq2 = 0;
            foreach (char c2 in richTextBox3.Text)
            {
                if (c2 == ch2)
                {
                    freq2++;
                }
            }
            label6.Text = $"@: ({freq2}/5)";
            if (freq2 > 5)
            {
                label6.ForeColor = Color.Red;
            }
            else
            {
                label6.ForeColor = Color.FromArgb(251, 173, 80);
            }
        }
    }
}
