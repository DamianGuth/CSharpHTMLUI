using IEFixLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpHTMLUI
{
    public class Browser
    {
        public static void Initialize()
        {
            string processName = Process.GetCurrentProcess().ProcessName + ".exe";

            BrowserSetup.SetupBrowser();
            BrowserSetup.SetupBrowser(processName);
            BrowserSetup.SetupBrowser(processName, 11);

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            Form1.webBrowser.Navigate(baseDirectory + "test.html");
            Form1.webBrowser.ObjectForScripting = new Logger();
        }
    }
}
