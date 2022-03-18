using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GramAIO
{  
    public partial class GramAIO : Form
    {
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        bool mouseDown;
        private Point offset;

        private int numCounter = 0;
        int placeCounter;
        int listElemCounter;
        private bool paused = false;
        public GramAIO()
        {
            InitializeComponent();
            pswTB.PasswordChar = '*';
            richTextBox1.Text = "Enter tags with or without '#', one per line";
            richTextBox2.Text = "Enter users with or without '@', one per line";
            richTextBox3.Text = "Enter comment to post. Only one comment allowed!";
            usrTB.ForeColor = Color.Green;
            pswTB.ForeColor = Color.Green;
            usrTB.Text = Properties.Settings.Default.username;
            pswTB.Text = Properties.Settings.Default.password;
            textBox1.Text = "5000";
            textBox2.Text = "5";
            loggingRichTextBox.ReadOnly = true;
            if(usrTB.Text == "" || pswTB.Text == "")
            {
                if(usrTB.Text == "")
                {
                    usrTB.ForeColor = Color.Red;
                    usrTB.Text = "USERNAME";
                }
                if(pswTB.Text == "")
                {
                    pswTB.ForeColor = Color.Red;
                    pswTB.Text = "PASSWORD";
                }
            }


            usrTB.SelectionStart = usrTB.TextLength;
            usrTB.SelectionLength = 0;
            pswTB.SelectionStart = pswTB.TextLength;
            pswTB.SelectionLength = 0;
            panel1.Select();
        }

        private void TextColor(RichTextBox box, string text, Color color)
        {
            if (loggingCheckbox.Checked)
            {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = 0;
                box.SelectionColor = color;
                box.AppendText("\n" + text);
                box.SelectionColor = box.ForeColor;
                box.ScrollToCaret();
            }
        }
        private void DelayTime()
        {
            TextColor(loggingRichTextBox, $"Delaying for {textBox1.Text}ms", Color.Orange);
            Thread.Sleep(Convert.ToInt32(textBox1.Text));           
        }
        List<string> listElem2 = new List<string>();
        List<string> listElem3 = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            if(checkBox4.Checked && checkBox3.Checked || checkBox4.Checked && checkBox2.Checked || checkBox4.Checked && checkBox1.Checked)
            {
                MessageBox.Show("You may only run the unfollow module while it is selected");
                return;
            }
            if (richTextBox1.Text != "Enter tags with or without '#', one per line" && richTextBox2.Text != "Enter users with or without '@', one per line")
            {
                MessageBox.Show("You may not run the tags and users module at the same time");
                return;
            }
            if(Convert.ToInt32(textBox1.Text) < 3000)
            {
                MessageBox.Show("WARNING your delay is under 3 seconds. This may result in performance errors.");
                return;
            }
            if (checkBox4.Checked == false && checkBox1.Checked == false && checkBox3.Checked == false && checkBox2.Checked == false)
            {
                MessageBox.Show("Please select a module to run first!");
                return;
            }

            if (label6.ForeColor == Color.Red || label5.ForeColor == Color.Red || label4.ForeColor == Color.Red)
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
            paused = false;
            TextColor(loggingRichTextBox, $"Logging in {usrTB.Text}", Color.Orange);

            List<string> list = new List<string>();            
            List<string> list2 = new List<string>();

            string linePlaceholder = "";
            if(richTextBox2.Text == "Enter users with or without '@', one per line")
            {
                foreach (string line in richTextBox1.Lines)
                {
                    linePlaceholder = line;
                    if (linePlaceholder.StartsWith("#"))
                    {
                        linePlaceholder = linePlaceholder.Remove(0, 1);
                    }
                    if (linePlaceholder.Contains(".com") || linePlaceholder.Contains("https:") || linePlaceholder.Contains("http:") || linePlaceholder.Contains("/"))
                    {
                        continue;
                    }
                    list.Add(linePlaceholder);
                }
            }
            else if (richTextBox2.Text != "Enter users with or without '@', one per line")
            {
                foreach (string line in richTextBox2.Lines)
                {
                    linePlaceholder = line;
                    if (linePlaceholder.StartsWith("@"))
                    {
                        linePlaceholder = linePlaceholder.Remove(0, 1);
                    }
                    if (linePlaceholder.Contains(".com") || linePlaceholder.Contains("https:") || linePlaceholder.Contains("http:") || linePlaceholder.Contains("/"))
                    {
                        continue;
                    }
                    list2.Add(linePlaceholder);
                }
            }
            new Task(() =>
            {
                ChromeOptions optionsRenew = new ChromeOptions();
                optionsRenew.AddArgument("--incognito");
                optionsRenew.AddArgument("--silent");

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

                    TextColor(loggingRichTextBox, "Logged in successfully", Color.Green);


                    // Logic part of program to handle module to run, all other code should be handled in their own classes

                    // users module
                    if (checkBox1.Checked && richTextBox2.Text != "Enter users with or without '@', one per line")
                    {
                        //TAG
                        numCounter = 0;
                        //POST
                        listElemCounter = 0;
                        //INTERACTIONS
                        placeCounter = 0;
                        async Task UsersModule()
                        {
                            while (numCounter < richTextBox2.Lines.Count())
                            {
                                try
                                {
                                    while (paused == true)
                                    {

                                    }
                                    List<string> listElem = new List<string>();
                                    DelayTime();
                                    TextColor(loggingRichTextBox, $"Getting user: {list2[numCounter]}", Color.Orange);
                                    driverRenew.Navigate().GoToUrl("https://www.instagram.com/" + list2[numCounter] + "/");
                                    TextColor(loggingRichTextBox, "Gathering user's followers", Color.Orange);
                                    w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("li.Y8-fY:nth-child(2) > a:nth-child(1) > div:nth-child(1)"))).Click();
                                    w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//a[@class='notranslate _0imsa ']")));
                                    var elem = driverRenew.FindElements(By.XPath("//a[@class='notranslate _0imsa ']"));
                                    int hrefCounter = 0;
                                    foreach (var elems in elem)
                                    {
                                        string ELEM = elems.GetAttribute("href");
                                        listElem.Add(ELEM);
                                        hrefCounter++;
                                    }
                                    TextColor(loggingRichTextBox, $"Total posts gathered: {hrefCounter}", Color.Green);
                                    DelayTime();

                                    void checkLink()
                                    {
                                        foreach (var elem3 in listElem3)
                                        {
                                            if (elem3 == listElem[listElemCounter])
                                            {
                                                TextColor(loggingRichTextBox, "Already took action on this user", Color.Red);
                                                TextColor(loggingRichTextBox, "Moving to next user", Color.Orange);
                                                listElemCounter++;
                                                checkLink();
                                            }
                                        }
                                    }

                                    checkLink();

                                    TextColor(loggingRichTextBox, $"Getting user: {listElem[listElemCounter]}", Color.Orange);
                                    driverRenew.Navigate().GoToUrl(listElem[listElemCounter]);
                                    listElem3.Add(listElem[listElemCounter]);
                                    w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//button[@class='sqdOP  L3NKy   y3zKF     ']")));
                                    DelayTime();
                                    driverRenew.FindElement(By.XPath("//button[@class='sqdOP  L3NKy   y3zKF     ']")).Click();
                                    TextColor(loggingRichTextBox, "Followed user successfully", Color.Green);
                                    DelayTime();
                                    driverRenew.Navigate().Refresh();

                                    if (checkBox2.Checked)
                                    {
                                        try
                                        {
                                            // gather posts from user
                                            w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//div[@class='Nnq7C weEfm']")));
                                            var postElem = driverRenew.FindElements(By.XPath("//div[@class='Nnq7C weEfm']"));
                                            List<string> userPostList = new List<string>();
                                            foreach (var item in postElem)
                                            {
                                                string ELEM = item.GetAttribute("href");
                                                userPostList.Add(ELEM);
                                                
                                            }
                                            driverRenew.Navigate().GoToUrl(userPostList[listElemCounter]);
                                            // find like button
                                            WebDriverWait wait = new WebDriverWait(driverRenew, TimeSpan.FromSeconds(5));
                                            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("button span > svg[aria-label='Like']")));
                                            //like the post
                                            DelayTime();
                                            var svgObject = driverRenew.FindElement(By.CssSelector("button span > svg[aria-label='Like']"));
                                            Actions builder = new Actions(driverRenew);
                                            builder.MoveToElement(svgObject).Click().Build().Perform();
                                            TextColor(loggingRichTextBox, "Liked post successfully", Color.Green);
                                        }
                                        catch (Exception)
                                        {
                                            TextColor(loggingRichTextBox, "Could not gather posts", Color.Red);
                                        }

                                    }
                                    TextColor(loggingRichTextBox, "Moving to next user", Color.Orange);
                                    listElemCounter++;
                                    placeCounter++;
                                    if (placeCounter == Convert.ToInt32(textBox2.Text))
                                    {
                                        TextColor(loggingRichTextBox, "Moving to next tag", Color.Orange);
                                        numCounter++;
                                        placeCounter = 0;
                                        listElemCounter = 0;
                                    }
                                    UsersModule();

                                }
                                catch (Exception)
                                {

                                }
                            }
                            //MessageBox.Show("No more tags to run");                               
                            driverRenew.Quit();
                            driverRenew.Close();
                        }
                        UsersModule();
                        TextColor(loggingRichTextBox, "All tags finished", Color.Green);
                        TextColor(loggingRichTextBox, "Stopping task", Color.Red);
                    }
                    if (checkBox1.Checked && richTextBox2.Text == "Enter users with or without '@', one per line")
                    {
                        TextColor(loggingRichTextBox, "Starting follow module", Color.Orange);
                        // Follow module

                        //TAG
                        numCounter = 0;
                        //POST
                        listElemCounter = 0;
                        //INTERACTIONS
                        placeCounter = 0;
                        //List<string> listElem2 = new List<string>();
                        async Task FollowModule()
                        {
                            while (numCounter < richTextBox1.Lines.Count())
                            {
                                if (checkBox5.Checked)
                                {
                                    if (listElemCounter < 9)
                                    {
                                        listElemCounter = 9;
                                        TextColor(loggingRichTextBox, "Smart mode (recent posts) enabled", Color.Green);
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
                                    DelayTime();
                                    TextColor(loggingRichTextBox, $"Getting tag: {list[numCounter]}", Color.Orange);
                                    driverRenew.Navigate().GoToUrl("https://www.instagram.com/explore/tags/" + list[numCounter] + "/");
                                    TextColor(loggingRichTextBox, "Gathering posts", Color.Orange);
                                    var elem = driverRenew.FindElements(By.XPath("//a[@href]"));
                                    int hrefCounter = 0;
                                    foreach (var elems in elem)
                                    {
                                        string ELEM = elems.GetAttribute("href");
                                        listElem.Add(ELEM);
                                        hrefCounter++;
                                    }
                                    TextColor(loggingRichTextBox, $"Total posts gathered: {hrefCounter}", Color.Green);
                                    DelayTime();
                                    if (listElem[listElemCounter] == null || listElem[listElemCounter] == "https://www.instagram.com/")
                                    {
                                        TextColor(loggingRichTextBox, "Tag does not contain sufficient posts", Color.Red);
                                        TextColor(loggingRichTextBox, "Getting next tag", Color.Orange);
                                        numCounter++;
                                        placeCounter = 0;
                                        FollowModule();
                                    }
                                    void checkLink()
                                    {
                                        foreach(var elem2 in listElem2)
                                        {
                                            if(elem2 == listElem[listElemCounter])
                                            {
                                                TextColor(loggingRichTextBox, "Already took action on this post", Color.Red);
                                                TextColor(loggingRichTextBox, "Moving to next post", Color.Orange);
                                                listElemCounter++;
                                                checkLink();
                                            }
                                        }
                                        /*
                                        if (listElem2.Contains(listElem[listElemCounter]))
                                        {
                                            TextColor(loggingRichTextBox, "Already took action on this post", Color.Red);
                                            TextColor(loggingRichTextBox, "Moving to next post", Color.Orange);
                                            listElemCounter++;
                                            checkLink();
                                        }
                                        */
                                    }
                                    checkLink();
                                    TextColor(loggingRichTextBox, $"Getting post: {listElem[listElemCounter]}", Color.Orange);
                                    driverRenew.Navigate().GoToUrl(listElem[listElemCounter]);
                                    listElem2.Add(listElem[listElemCounter]);
                                    w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("button.sqdOP:nth-child(2) > div:nth-child(1)")));
                                    DelayTime();
                                    driverRenew.FindElement(By.CssSelector("button.sqdOP:nth-child(2) > div:nth-child(1)")).Click();
                                    TextColor(loggingRichTextBox, "Followed user successfully", Color.Green);
                                    DelayTime();
                                    driverRenew.Navigate().Refresh();
                                    if (checkBox2.Checked)
                                    {
                                        //like the post
                                        //w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("/html/body/div[1]/section/main/div/div[1]/article/div/div[2]/div/div[2]/section[1]/span[1]/button/div[1]/svg")));
                                        DelayTime();
                                        var svgObject = driverRenew.FindElement(By.CssSelector("button span > svg[aria-label='Like']"));
                                        Actions builder = new Actions(driverRenew);
                                        builder.MoveToElement(svgObject).Click().Build().Perform();
                                        TextColor(loggingRichTextBox, "Liked post successfully", Color.Green);
                                    }

                                    if (checkBox3.Checked)
                                    {
                                        //comment on post

                                        DelayTime();
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

                                        DelayTime();
                                        var svgObject2 = driverRenew.FindElement(By.XPath("/html/body/div[1]/section/main/div/div[1]/article/div/div[2]/div/div[2]/section[3]/div/form/button/div"));
                                        Actions builder2 = new Actions(driverRenew);
                                        builder2.MoveToElement(svgObject2).Click().Build().Perform();
                                    }
                                    TextColor(loggingRichTextBox, "Moving to next post", Color.Orange);
                                    listElemCounter++;
                                    placeCounter++;
                                    if (placeCounter == Convert.ToInt32(textBox2.Text))
                                    {
                                        TextColor(loggingRichTextBox, "Moving to next tag", Color.Orange);
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
                            //MessageBox.Show("No more tags to run");                               
                            driverRenew.Quit();
                            driverRenew.Close();
                        }
                        FollowModule();
                        TextColor(loggingRichTextBox, "All tags finished", Color.Green);
                        TextColor(loggingRichTextBox, "Stopping task", Color.Red);
                    }

                    //////////////////////////////////////////
                    else if (checkBox2.Checked && richTextBox2.Text == "Enter users with or without '@', one per line")
                    {
                        TextColor(loggingRichTextBox, "Starting like module", Color.Orange);
                        //TAG
                        numCounter = 0;
                        //POST
                        listElemCounter = 0;
                        //INTERACTIONS
                        placeCounter = 0;

                        async Task LikeModule()
                        {
                            while (numCounter < richTextBox1.Lines.Count())
                            {

                                if (checkBox5.Checked)
                                {
                                    if (listElemCounter < 9)
                                    {
                                        listElemCounter = 9;
                                        TextColor(loggingRichTextBox, "Smart mode (recent posts) enabled", Color.Green);
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
                                    DelayTime();
                                    TextColor(loggingRichTextBox, $"Getting tag: {list[numCounter]}", Color.Orange);
                                    driverRenew.Navigate().GoToUrl("https://www.instagram.com/explore/tags/" + list[numCounter] + "/");
                                    TextColor(loggingRichTextBox, "Gathering posts", Color.Orange);
                                    var elem = driverRenew.FindElements(By.XPath("//a[@href]"));
                                    int hrefCounter = 0;
                                    foreach (var elems in elem)
                                    {
                                        string ELEM = elems.GetAttribute("href");
                                        listElem.Add(ELEM);
                                        hrefCounter++;
                                    }
                                    TextColor(loggingRichTextBox, $"Total posts gathered: {hrefCounter}", Color.Green);
                                    DelayTime();
                                    if (listElem[listElemCounter] == null || listElem[listElemCounter] == "https://www.instagram.com/")
                                    {
                                        TextColor(loggingRichTextBox, "Tag does not contain sufficient posts", Color.Red);
                                        TextColor(loggingRichTextBox, "Getting next tag", Color.Orange);
                                        numCounter++;
                                        placeCounter = 0;
                                        LikeModule();
                                    }
                                    void checkLink()
                                    {
                                        foreach (var elem2 in listElem2)
                                        {
                                            if (elem2 == listElem[listElemCounter])
                                            {
                                                TextColor(loggingRichTextBox, "Already took action on this post", Color.Red);
                                                TextColor(loggingRichTextBox, "Moving to next post", Color.Orange);
                                                listElemCounter++;
                                                checkLink();
                                            }
                                        }
                                        /*
                                        if (listElem2.Contains(listElem[listElemCounter]))
                                        {
                                            TextColor(loggingRichTextBox, "Already took action on this post", Color.Red);
                                            TextColor(loggingRichTextBox, "Moving to next post", Color.Orange);
                                            listElemCounter++;
                                            checkLink();
                                        }
                                        */
                                    }
                                    checkLink();
                                    TextColor(loggingRichTextBox, $"Getting post: {listElem[listElemCounter]}", Color.Orange);
                                    driverRenew.Navigate().GoToUrl(listElem[listElemCounter]);
                                    listElem2.Add(driverRenew.Url);
                                    DelayTime();
                                    var svgObject = driverRenew.FindElement(By.CssSelector("button span > svg[aria-label='Like']"));
                                    Actions builder = new Actions(driverRenew);
                                    builder.MoveToElement(svgObject).Click().Build().Perform();
                                    TextColor(loggingRichTextBox, "Liked post successfully", Color.Green);

                                    if (checkBox1.Checked)
                                    {
                                        //follow the user
                                        w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("button.sqdOP:nth-child(2) > div:nth-child(1)")));
                                        DelayTime();
                                        driverRenew.FindElement(By.CssSelector("button.sqdOP:nth-child(2) > div:nth-child(1)")).Click();
                                        TextColor(loggingRichTextBox, "Followed user successfully", Color.Green);
                                        DelayTime();
                                        driverRenew.Navigate().Refresh();
                                    }
                                    TextColor(loggingRichTextBox, "Moving to next post", Color.Orange);
                                    listElemCounter++;
                                    placeCounter++;
                                    if (placeCounter == Convert.ToInt32(textBox2.Text))
                                    {
                                        TextColor(loggingRichTextBox, "Moving to next tag", Color.Orange);
                                        numCounter++;
                                        placeCounter = 0;
                                        listElemCounter = 0;
                                    }
                                    LikeModule();
                                }
                                catch (Exception)
                                {

                                }
                            }
                            //MessageBox.Show("No more tags to run");         
                            driverRenew.Quit();
                            driverRenew.Close();
                        }
                        LikeModule();
                        TextColor(loggingRichTextBox, "All tags finished", Color.Green);
                        TextColor(loggingRichTextBox, "Stopping task", Color.Red);
                    }
                    else if (checkBox3.Checked)
                    {
                        // Comment module
                        TextColor(loggingRichTextBox, "Starting comment module", Color.Green);
                    }
                    else if (checkBox4.Checked)
                    {
                        w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("/html/body/div[1]/section/main/div/header/section/ul/li[3]/a"))).Click();
                        w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//button[@class='sqdOP  L3NKy    _8A5w5    ']")));
                        var users = driverRenew.FindElements(By.XPath("//button[@class='sqdOP  L3NKy    _8A5w5    ']"));
                        foreach(var user in users)
                        {
                            user.Click();
                            w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//button[@class='aOOlW -Cab_   ']"))).Click();
                            DelayTime();
                        }
                    }

                    // code below no longer needed
                    /*
                    else if (checkBox1.Checked && checkBox2.Checked && checkBox3.Checked)
                    {
                        // AIO module
                    }
                    */

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

        private void usrTB_Click(object sender, EventArgs e)
        {
            if(usrTB.Text == "USERNAME")
            {
                usrTB.Text = "";
            }       
        }

        private void pswTB_Click(object sender, EventArgs e)
        {
            if(pswTB.Text == "PASSWORD")
            {
                pswTB.Text = "";
            }     
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TextColor(loggingRichTextBox, "Resetting", Color.Orange);
            numCounter = 0;
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
            TextColor(loggingRichTextBox, "Reset complete", Color.Green);
            numCounter = 0;
            placeCounter = 0;
            listElemCounter = 0;
            paused = true;
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
            else
            {
                Properties.Settings.Default.username = usrTB.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void pswTB_Leave(object sender, EventArgs e)
        {
            if (pswTB.Text == "")
            {
                pswTB.Text = "PASSWORD";
            }
            else
            {
                Properties.Settings.Default.password = pswTB.Text;
                Properties.Settings.Default.Save();
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
                richTextBox1.Text = "Enter tags with or without '#', one per line";
            }
        }

        private void richTextBox2_Leave(object sender, EventArgs e)
        {
            if (richTextBox2.Text == "")
            {
                richTextBox2.Text = "Enter users with or without '@', one per line";
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
                TextColor(loggingRichTextBox, "Pausing tasks, please wait", Color.Orange);
            }
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "Enter tags with or without '#', one per line")
            {
                richTextBox1.Text = "";
            }
        }

        private void richTextBox2_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text == "Enter users with or without '@', one per line")
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
            if (chars > 2200)
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
            if (freq > 30)
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

        private void label7_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            mouseDown = true;
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void usrTB_TextChanged(object sender, EventArgs e)
        {
            if (usrTB.Text == "USERNAME")
            {
                usrTB.ForeColor = Color.Red;
            }
            else
            {
                usrTB.ForeColor = Color.Green;
            }
        }

        private void pswTB_TextChanged(object sender, EventArgs e)
        {
            if (pswTB.Text == "PASSWORD")
            {
                pswTB.ForeColor = Color.Red;
            }
            else
            {
                pswTB.ForeColor = Color.Green;
            }
        }

        private void loggingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (loggingCheckbox.Checked)
            {
                this.Width = 1100;
                label7.Location = new Point(1072, 12);
                loggingRichTextBox.Visible = true;
            }
            else
            {
                this.Width = 816;
                label7.Location = new Point(793, 12);
                loggingRichTextBox.Visible = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            loggingRichTextBox.Text = null;
        }

        private void label2_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(label2, @"Instagram hashtags that you wish to use.");
        }

        private void label3_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(label3, @"Instagram usernames that you wish to use.");
        }

        private void usrTB_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(usrTB, "Instagram username");
        }

        private void pswTB_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pswTB, "Instagram password");
        }
    }
}
