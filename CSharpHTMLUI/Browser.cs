using IEFixLib;
using System;
using System.Diagnostics;
using System.IO;
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
            LoadPages();

            // Visit the index page
            Form1.webBrowser.Navigate(baseDirectory + "F/index.html");
            Form1.webBrowser.ObjectForScripting = new HTMLBridge();
        }

        private static void LoadPages()
        {
            // Need a way to get the name of the file. That way the file could be re-generated safely

            //var assembly = Assembly.GetExecutingAssembly();

            //foreach (var resourceName in assembly.GetManifestResourceNames())
            //    System.Console.WriteLine("ASM:" + resourceName);

            //foreach (var resourceName in assembly.GetManifestResourceNames())
            //{
            //    using (var stream = assembly.GetManifestResourceStream(resourceName))
            //    {
            //        StreamReader streamReader = new StreamReader(stream);
            //        string html = streamReader.ReadToEnd();
            //        Console.WriteLine("MYRES:" + html);
            //        Console.WriteLine("END MY RES");
            //        //      string html = streamReader.ReadToEnd();
            //        File.WriteAllText(resourceName + "1", html);
            //        Console.WriteLine("");
            //    }
            //}
            if (!Directory.Exists("F/"))
                Directory.CreateDirectory("F/");

            File.WriteAllText("F/index.html", Generic.MinifyHTML(CSharpHTMLUI.Properties.Resources.index));
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


        /// <summary>
        /// Sets the text of an HTML element
        /// </summary>
        /// <param name="elementID"></param>
        /// <param name="text"></param>
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

    }
}
