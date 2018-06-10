using IEFixLib;
using mshtml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CSharpHTMLUI
{
    public class Browser
    {
        private static List<CachedPage> CachedPages = new List<CachedPage>();

        public struct CachedPage
        {
            public string name;
            public string html;
        };

        public static void Initialize()
        {
            string processName = Process.GetCurrentProcess().ProcessName + ".exe";

            BrowserSetup.SetupBrowser();
            BrowserSetup.SetupBrowser(processName);
            BrowserSetup.SetupBrowser(processName, 11);

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            LoadPages();

            // Visit the index page
            Form1.webBrowser.Navigate(baseDirectory + "F/indexKopie.html");
            Form1.webBrowser.ObjectForScripting = new HTMLBridge();
        }

        private static void LoadPages()
        {
            int count = 0;

            if (Directory.Exists("F/"))
            {
                Directory.Delete("F/", true);
                Directory.CreateDirectory("F/");
            }

            if (!Directory.Exists("F/"))
            {
                Directory.CreateDirectory("F/");
                System.Threading.Thread.Sleep(400);
            }

            var assembly = Assembly.GetExecutingAssembly();

            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                count++;
                if (count == 1)
                    continue;

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    StreamReader streamReader = new StreamReader(stream);

                    string content = streamReader.ReadToEnd();

                    string[] files = content.Split(new string[] { "<!--EOF-->" }, StringSplitOptions.None);


                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i] == "")
                            continue;

                        string html = "";
                        string fileName = "";

                        // Get file lines
                        string[] splitLines = files[i].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                        int splitIndex = (i == 0) ? 1 : 0;

                        // Get the first line
                        string[] firstLine = splitLines[splitIndex].Split(':');

                        // Set the filename
                        if (firstLine != null && firstLine.Length >= 2)
                        {
                            fileName = firstLine[1];
                        }

                        int yIndex = (i == 0) ? 1 : 0;

                        // For each line of the file
                        for (int y = 0; y < splitLines.Length; y++)
                        {
                            if (y > yIndex)
                            {
                                html += splitLines[y];
                            }
                        }

                        // Create a new cachedpage for the current page
                        CachedPage page = new CachedPage();
                        page.name = fileName;
                        page.html = html;

                        // Add the page to the cache
                        CachedPages.Add(page);

                        // Wait for windows to create the folder...
                        if (!Directory.Exists("F/"))
                        {
                            System.Threading.Thread.Sleep(400);
                        }

                        // Create the file
                        File.WriteAllText("F/" + fileName, Generic.MinifyHTML(html));
                    }
                }
            }
        }

        /// <summary>
        /// Loads a cached page by name
        /// </summary>
        /// <param name="name">The name of the page to load (must contain file endings!)</param>
        public static void LoadCachedPage(string name)
        {
            // Find the correct page
            foreach (CachedPage cp in CachedPages)
            {
                if (cp.name == name)
                {
                    Form1.webBrowser.DocumentText = "0";
                    Form1.webBrowser.Document.OpenNew(true);
                    Form1.webBrowser.Document.Write(cp.html);
                    Form1.webBrowser.Refresh();

                    // Exit the loop
                    break;
                }
            }
        }

        /// <summary>
        /// Injects all base methods into the current page
        /// </summary>
        public static void InjectMethods()
        {
            // Inject the method to get the text of an item
            HtmlElement htmlHead = Form1.webBrowser.Document.GetElementsByTagName("head")[0];
            HtmlElement getInnerTextScript = Form1.webBrowser.Document.CreateElement("script");
            IHTMLScriptElement element = (IHTMLScriptElement)getInnerTextScript.DomElement;
            element.text = "function getInnerText(id) {return document.getElementById(id).value;}";
            htmlHead.AppendChild(getInnerTextScript);
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

        /// <summary>
        /// Gets the text of an element
        /// </summary>
        /// <param name="id">The id of the element to get the text from</param>
        /// <returns>The text of the element</returns>
        public static string GetElementText(string id)
        {
            return (string)Form1.webBrowser.Document.InvokeScript("getInnerText", new string[] { id });
        }

    }
}
