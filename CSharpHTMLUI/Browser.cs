using IEFixLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            // Visit the index page
            Form1.webBrowser.Navigate(baseDirectory + "index.html");
            Form1.webBrowser.ObjectForScripting = new HTMLBridge();
        }

        public static void SetText(string elementID, string text)
        {
            try
            {
                Form1.webBrowser.Document.GetElementById(elementID).InnerText = text;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Generates an HTML element
        /// </summary>
        /// <param name="type">The type of the element</param>
        /// <param name="id">The id that element should get. If this field is empty a random id will be generated</param>
        public static void GenerateElement(string type, string elementText = "", string appendToId = "", string id = "")
        {
            if (id == "")
                id = Generic.GetRandomString(Generic.IDLength);

            if (appendToId == "")
                appendToId = "body";

            HtmlElement element = Form1.webBrowser.Document.CreateElement(type);
            element.Id = id;

            if (elementText != "")
                element.InnerText = elementText;

            Form1.webBrowser.Document.GetElementById(appendToId).AppendChild(element);
        }
    }
}
