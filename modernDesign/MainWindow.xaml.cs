using modernDesign.Functions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using System.Windows.Threading;
using System.Xml.Linq;

namespace modernDesign
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static private string filename = null;
        static private bool runHeadless = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        [STAThread]
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            runHeadless = Convert.ToBoolean(runHeadlessChecker.IsChecked); //Check to run headless or not
            //Thread t = new Thread(() =>
            //{
                    if (filename == null)
                    {
                        fileSelecter.Text = "!SELECT FILE!";
                    }
                    else
                    {

                        //-------------------------------------Initialize---------------------------------------

                        //Define path to tesseract.exe
                        string path_to_tesseract = @"C:\Program Files\Tesseract-OCR\tesseract.exe";

                        //Define path to tesseract.exe
                        string path_to_image = "captcha.png";


                        const string primeURL = "https://gaming.amazon.com/oauth/start/riot?overwrite=true&redirectUrl=https://gaming.amazon.com/loot/lol10";

                        string lol_uname = null;
                        string lol_pass = null;
                        string prime_mail = null;
                        string prime_pass = null;
                        var autherror = false;
                        int error_count = 0;
                        int succes_count = 0;
                        int no_prime_count = 0;
                        bool phone_skipper = false;
                        bool phone_skipper2 = false; // NEW VARIABLE

                        int teller = 0;

                        TextBlock tb = new TextBlock();


                        //-------------------------------------Main Code---------------------------------------

                        //Bool true als user programma mag gebruiken
                        bool isValidUser = true;

                        //User HWID comes here
                        string HWID = "03000200-0400-0500-0006-000700080009";

                        //Pull HWID list from DB (Of check gewoon of HWID in DB zit en mag gebruikt worden)
                        string HWID_List = "";


                        try
                        {
                            if (isValidUser) //User heeft shmoney betaald
                            {


                                string[] lines = File.ReadAllLines(filename); //String array van txt file

                                foreach (string line in lines)
                                {
                                    var seleniumOptions = new ChromeOptions(); //Options aanmaken
                                    seleniumOptions.AddArguments("--incognito");
                                    seleniumOptions.AddArguments("--window-size=1920x1080");
                                    //seleniumOptions.AddArguments("--start-maximized");
                                    seleniumOptions.AddArguments("--log-level=3");

                                    //seleniumOptions.AddArguments("--headless");

                                    IWebDriver driver = new ChromeDriver(seleniumOptions); //Startup
                                    try
                                    {
                                        teller++;

                                        lol_uname = line.Split(':')[0];
                                        lol_pass = line.Split(':')[1];
                                        prime_mail = line.Split(':')[2];
                                        prime_pass = line.Split(':')[3];

                                        Debug.WriteLine(teller);
                                        Debug.WriteLine("Username: " + lol_uname);
                                        //Debug.WriteLine("Password: " + lol_pass);
                                        Debug.WriteLine("Email: " + prime_mail);
                                        //Debug.WriteLine("Pass: " + prime_pass);

                                        if (runHeadless)
                                        {
                                            seleniumOptions.AddArguments("--headless");
                                        }

                                        //seleniumOptions.AddArguments("--headless");


                                        // ----------------------------------- MAIN PROGRAM -------------------------------------
                                        Debug.WriteLine("Logging in to amazon..");
                                        driver.Navigate().GoToUrl(primeURL);

                                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

                                        tb.Text = "Logging in";
                                        tb.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCCFC0"));
                                        tb.FontSize = 16;
                                        theBox3.Items.RemoveAt(teller - 1);
                                        theBox3.Items.Insert((teller - 1), tb);

                                        driver.FindElement(By.XPath("/html/body/div[2]/div/div/main/div/div/div/div[2]/div[3]/button"), 10).Click();

                                        // -------------- REMOVED
                                        // Amazon prime claim button
                                        //driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/main/div/div/div/div[2]/div[2]/div[1]/div/div[1]/div/div/button/span/div"), 10).Click();

                                        // Amazon prime Sign in button
                                        //driver.FindElement(By.XPath("/html/body/div[4]/div/div/div[1]/div/div/div[3]/div/div/div[3]/div/div[1]/button/span"), 10).Click();
                                        // -----------------------

                                        // Amazon email field
                                        var element = driver.FindElement(By.Id("ap_email"), 10);
                                        element.Click();
                                        element.SendKeys(prime_mail);

                                        // Amazon password field
                                        element = driver.FindElement(By.Id("ap_password"), 10);
                                        element.Click();
                                        element.SendKeys(prime_pass);

                                        // Press sign in button
                                        driver.FindElement(By.Id("signInSubmit")).Click();

                                        // Additional login checks

                                        // Volgorde warning box a --> error message --> warning box

                                        // Login Captcha na login
                                        // Eerste captcha handling
                                        try
                                        {
                                            var employeeLabel = driver.FindElement(By.XPath("//*[@id=\"auth-warning-message-box\"]/div/h4"), 1);
                                            if (employeeLabel != null)
                                            {
                                                autherror = true;
                                            }
                                        }
                                        catch
                                        {
                                            autherror = false;
                                        }
                                        // Pass opnieuw invullen en wachten op captcha
                                        if (autherror)
                                        {
                                            Debug.WriteLine("Need to solve captcha");
                                            tb.Text = "Captcha";
                                            tb.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCCFC0"));
                                            tb.FontSize = 16;
                                            theBox3.Items.RemoveAt(teller - 1);
                                            theBox3.Items.Insert((teller - 1), tb);
                                            while (autherror)
                                            {
                                                element = driver.FindElement(By.Id("ap_password"), 2);
                                                // Check if password was filled in if not click on it and fill in
                                                try
                                                {

                                                    if (element.GetAttribute("value") == "" && autherror == true)
                                                    {
                                                        element.Click();
                                                        element.SendKeys(prime_pass);
                                                        //TODO: captcha solving
                                                    }
                                                }
                                                catch
                                                {
                                                    autherror = false;
                                                }
                                                try
                                                {
                                                    var employeeLabel = driver.FindElement(By.XPath("//*[@id=\"auth-warning-message-box\"]/div/h4"));
                                                    if (employeeLabel != null)
                                                    {
                                                        autherror = true;
                                                    }
                                                    else
                                                    {
                                                        autherror = false;
                                                    }
                                                }
                                                catch
                                                {
                                                    autherror = false;
                                                }
                                                Debug.WriteLine(autherror);
                                                Thread.Sleep(500);
                                            }
                                        }

                                        // Wrong pass/locked 
                                        try
                                        {
                                            var employeeLabel = driver.FindElement(By.XPath("//*[@id=\"auth-error-message-box\"]/div/h4"), 1);
                                            if (employeeLabel != null)
                                            {
                                                autherror = true;
                                            }
                                        }
                                        catch
                                        {
                                            autherror = false;
                                        }

                                        if (autherror)
                                        {
                                            Debug.WriteLine("Account locked or invalid.");
                                            tb.Text = "Invalid";
                                            tb.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCCFC0"));
                                            tb.FontSize = 16;
                                            theBox3.Items.RemoveAt(teller - 1);
                                            theBox3.Items.Insert((teller - 1), tb);
                                            error_count++;

                                            using (StreamWriter w = File.AppendText("error.txt"))
                                            {
                                                w.WriteLine($"{lol_uname}:{lol_pass}:{prime_mail}:{prime_pass}");
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine("Logged in!");
                                            // First check for activate prime button
                                            // Check if riot login page is there if not go through process
                                            // Then if there's none check for phone skip this is to improve speed
                                            phone_skipper = false;

                                            try
                                            {
                                                var activePresent = driver.FindElement(By.Name("username"), 3);
                                                if (activePresent != null)
                                                {
                                                    phone_skipper = true;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                Debug.WriteLine("Continue");
                                            }

                                            if (phone_skipper == false)
                                            {
                                                phone_skipper2 = false;
                                                try
                                                {
                                                    var activePresent = driver.FindElement(By.XPath("/html/body/div[4]/div/div/div[1]/div/div/div[3]/div/div/div[3]/div/div/button"), 1);
                                                    if (activePresent != null)
                                                    {
                                                        Debug.WriteLine("Pressing activate skip");
                                                        driver.FindElement(By.XPath("/html/body/div[4]/div/div/div[1]/div/div/div[3]/div/div/div[3]/div/div/button"), 2).Click();
                                                        phone_skipper2 = true;
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    Debug.WriteLine("Continue");
                                                }

                                                // Activate skip 
                                                if (phone_skipper2 == false)
                                                {
                                                    try
                                                    {
                                                        var phonePresent = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div[2]/div/div/div/form/div/div[5]/div/a"), 3); //Check if phone skip is present
                                                        if (phonePresent != null)
                                                        {
                                                            Debug.WriteLine("Pressing phone skip");
                                                            driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div[2]/div/div/div/form/div/div[5]/div/a"), 2).Click();
                                                        }
                                                        else
                                                        {
                                                            Debug.WriteLine("No phone skip");
                                                        }
                                                    }
                                                    catch (Exception es)
                                                    {
                                                        Debug.WriteLine("Error: " + es);
                                                    }

                                                    try
                                                    {
                                                        var activePresent = driver.FindElement(By.XPath("/html/body/div[4]/div/div/div[1]/div/div/div[3]/div/div/div[3]/div/div/button"), 2);
                                                        if (activePresent != null)
                                                        {
                                                            Debug.WriteLine("Pressing activate skip");
                                                            driver.FindElement(By.XPath("/html/body/div[4]/div/div/div[1]/div/div/div[3]/div/div/div[3]/div/div/button"), 2).Click();
                                                        }
                                                        else
                                                        {
                                                            Debug.WriteLine("No active skip");
                                                        }
                                                    }
                                                    catch (Exception es)
                                                    {
                                                        Debug.WriteLine("Error: " + es);
                                                    }
                                                }
                                            }

                                            bool hasPrime = false;

                                            if (driver.FindElement(By.Name("username"), 15) != null) //check for prime
                                            {
                                                Debug.WriteLine("Starting redeeming process.");

                                                hasPrime = true;

                                                /* ----------------- GONE
                                                Debug.WriteLine("hasPrime: " + hasPrime);
                                                //continue button
                                                driver.FindElement(By.XPath("//p[contains(text(), 'Continue')]")).Click();
                                                // Go to riot games
                                                driver.FindElement(By.XPath("/html/body/div[4]/div/div/div[1]/div/div/div[3]/div[2]/div/div[2]/div/div/div/button/span"), 10).Click();
                                                */

                                                Debug.WriteLine("Logging in to riot.");

                                                tb.Text = "Riot Login";
                                                tb.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCCFC0"));
                                                tb.FontSize = 16;
                                                theBox3.Items.RemoveAt(teller - 1);
                                                theBox3.Items.Insert((teller - 1), tb);
                                                //riot login

                                                try
                                                {
                                                    element = driver.FindElement(By.Name("username"), 10);
                                                    element.Click();
                                                    element.SendKeys(lol_uname);

                                                    element = driver.FindElement(By.Name("password"), 10);
                                                    // Press tab otherwise cookies in the way
                                                    element.SendKeys(Keys.Tab);
                                                    element.SendKeys(lol_pass);
                                                    element.SendKeys(Keys.Enter);
                                                }
                                                catch (Exception es)
                                                {
                                                    Debug.WriteLine(es);
                                                }


                                                try
                                                {
                                                    // Check for authorize button
                                                    driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/div/div/div[1]/button"), 5).Click();
                                                }
                                                catch (Exception)
                                                {
                                                    Debug.WriteLine("No Auth button");
                                                }

                                                // Complete claim ------------------ CHANGED XPATH
                                                Thread.Sleep(1000);
                                                driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div/main/div/div/div/div[2]/div[2]/div[1]/div/div[1]/div/div/button/span/div/div[2]"), 5).Click();

                                                try
                                                {
                                                    driver.FindElement(By.XPath("/html/body/div[4]/div/div/div[1]/div/div/div[3]/div[3]/div/div[1]/div/button"), 1).Click();
                                                }
                                                catch
                                                {
                                                    Debug.WriteLine("Continue");
                                                }


                                                // SUCCESS
                                                tb.Text = "Succes";
                                                tb.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCCFC0"));
                                                tb.FontSize = 16;
                                                theBox3.Items.RemoveAt(teller - 1);
                                                theBox3.Items.Insert((teller - 1), tb);
                                                using (StreamWriter w = File.AppendText("succes.txt"))
                                                {
                                                    Debug.WriteLine("---------------Succesfully claimed---------------"); // voor debug
                                                    w.WriteLine($"{lol_uname}:{lol_pass}:{prime_mail}:{prime_pass}");
                                                }

                                                // Debug.ReadLine(); // debug

                                            }
                                            else
                                            {
                                                using (StreamWriter w = File.AppendText("no_prime.txt"))
                                                {
                                                    w.WriteLine($"{lol_uname}:{lol_pass}:{prime_mail}:{prime_pass}");
                                                }
                                            }
                                        }


                                    }
                                    catch (Exception es)
                                    {
                                        Debug.WriteLine("Error occured Skipping account");
                                        tb.Text = "Error";
                                        tb.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCCFC0"));
                                        tb.FontSize = 16;
                                        theBox3.Items.RemoveAt(teller - 1);
                                        theBox3.Items.Insert((teller - 1), tb);
                                        Debug.WriteLine(es);
                                        using (StreamWriter w = File.AppendText("error.txt"))
                                        {
                                            w.WriteLine($"{lol_uname}:{lol_pass}:{prime_mail}:{prime_pass}");
                                        }

                                    }
                                    driver.Quit();
                                }
                            }
                            else
                            {
                                Debug.WriteLine("Subscription expired");
                            }
                        }
                        catch (Exception es)
                        {
                            Console.WriteLine("ERROR " + es);
                        }
                    }
            //});
            //t.SetApartmentState(ApartmentState.STA);
            //t.IsBackground = true;
            //t.Start();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                filename = dialog.FileName;
                string[] strlist = filename.Split('\\'); //Split by backslash
                fileSelecter.Text = strlist[strlist.Length-1]; //Alleen filename laten zien, niet hele path

                //--------SHOWING ACC'S IN ON MAIN WINDOW---------
                string[] lines = File.ReadAllLines(filename);

                foreach(string line in lines)
                {
                    string lol_uname = line.Split(':')[0];
                    string lol_pass = line.Split(':')[1];
                    string prime_mail = line.Split(':')[2];
                    string prime_pass = line.Split(':')[3];

                    TextBlock tb = new TextBlock();
                    tb.Text = lol_uname + ":" + lol_pass;
                    tb.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCCFC0"));
                    tb.FontSize = 16;
                    theBox.Items.Add(tb);

                    TextBlock tb2 = new TextBlock();
                    tb2.Text = prime_mail + ":" + prime_pass;
                    tb2.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCCFC0"));
                    tb2.FontSize = 16;
                    theBox2.Items.Add(tb2);

                    TextBlock tb3 = new TextBlock();
                    tb3.Text = "Waiting...";
                    tb3.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCCFC0"));
                    tb3.FontSize = 16;
                    theBox3.Items.Add(tb3);
                    //theBox3.Items.Insert();
                }
            }
        }

        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            Window1 window = new Window1();
            window.Show();
        }
    }
}
