using IEFixLib;
using mshtml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace CSharpHTMLUI
{
    public class Renderer
    {
        private static List<CachedPage> ResourceLoadedPages = new List<CachedPage>();
        public static List<CachedPage> CachedPages = new List<CachedPage>();
        public static CachedPage CurrentPage = new CachedPage();

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
            Form1.webBrowser.Navigate(baseDirectory + "F/index.html");
            CurrentPage.name = "index.html";
            CurrentPage.html = Form1.webBrowser.DocumentText;
            Form1.webBrowser.ObjectForScripting = new HTMLBridge();

          //  File.WriteAllText("F/jquery.min.js", Properties.Resources.jquery);
            File.WriteAllText("F/jquery.min.js", JQueryHandle.jquery);
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

                        int splitIndex = (i == 0) ? 1 : 1;

                        // Get the first line
                        string[] firstLine = splitLines[splitIndex].Split(':');

                        // Set the filename
                        if (firstLine != null && firstLine.Length >= 2)
                        {
                            fileName = firstLine[1];
                        }

                        int yIndex = (i == 0) ? 1 : 1;

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
                        ResourceLoadedPages.Add(page);

                        // Wait for windows to create the folder...
                        if (!Directory.Exists("F/"))
                        {
                            System.Threading.Thread.Sleep(400);
                        }

                        // Create the file
                        if(fileName != "" && fileName != "function(a,b){M.remove(a,b)},_data")
                        File.WriteAllText("F/" + fileName, Generic.MinifyHTML(html));
                    }
                }
            }
            CachedPages = ResourceLoadedPages;
        }

        /// <summary>
        /// Loads a cached page by name
        /// </summary>
        /// <param name="name">The name of the page to load (must contain file endings!)</param>
        public static void LoadCachedPage(string name)
        {
            UpdateLocalCache();
            // Find the correct page
            foreach (CachedPage cp in CachedPages)
            {
                if (cp.name == name)
                {
                    Form1.webBrowser.DocumentText = "0";
                    Form1.webBrowser.Document.OpenNew(true);
                    Form1.webBrowser.Document.Write(cp.html);
                    Form1.webBrowser.Refresh();

                    CurrentPage.name = name;
                    CurrentPage.html = cp.html;
                    // Exit the loop
                    break;
                }
            }
        }

        /// <summary>
        /// Loads a page from the resources
        /// </summary>
        /// <param name="name">The name of the page to load (must contain file endings!)</param>
        public static void LoadPageFromResources(string name)
        {
            UpdateLocalCache();

            // Find the correct page
            foreach (CachedPage cp in ResourceLoadedPages)
            {
                if (cp.name == name)
                {
                    Form1.webBrowser.DocumentText = "0";
                    Form1.webBrowser.Document.OpenNew(true);
                    Form1.webBrowser.Document.Write(cp.html);
                    Form1.webBrowser.Refresh();

                    CurrentPage.name = name;
                    CurrentPage.html = cp.html;

                    // Exit the loop
                    break;
                }
            }
        }

        /// <summary>
        /// Updates the cache of the current page
        /// </summary>
        private static void UpdateLocalCache()
        {
            if (CurrentPage.name == null || CurrentPage.name == "")
                return;

            CurrentPage.html = Form1.webBrowser.Document.GetElementsByTagName("html")[0].OuterHtml;

            // Find the correct page and update it
            for (int i = 0; i < CachedPages.Count; i++)
            {
                if (CachedPages[i].name == CurrentPage.name)
                {
                    CachedPages.RemoveAt(i);
                    CachedPage tmpPage = new CachedPage();
                    tmpPage.name = CurrentPage.name;
                    tmpPage.html = CurrentPage.html;
                    CachedPages.Add(tmpPage);
                    break;
                }
            }
        }

        /// <summary>
        /// Updates the cache for a specific page without updateing the current page
        /// </summary>
        /// <param name="name">The name of the page (must contain file endings!)</param>
        /// <param name="html">The html content to update the cache with</param>
        public static void UpdateCache(string name, string html)
        {
            // Find the correct page and update it
            for (int i = 0; i < CachedPages.Count; i++)
            {
                if (CachedPages[i].name == name)
                {
                    CachedPages.RemoveAt(i);
                    CachedPage tmpPage = new CachedPage();
                    tmpPage.name = name;
                    tmpPage.html = html;
                    CachedPages.Add(tmpPage);
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

            //HtmlElement innerJquery = Form1.webBrowser.Document.CreateElement("script");
            //IHTMLScriptElement jqueryElement = (IHTMLScriptElement)innerJquery.DomElement;
            //jqueryElement.text = Properties.Resources.jquery;

            //Assembly assembly = this.GetType().Assembly;
            //ResourceManager resourceManager = new ResourceManager("Resources.Strings", assembly);
            // string myString = resourceManager.GetString("value");


            //htmlHead.AppendChild(innerJquery);

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

        /// <summary>
        /// Cleans up data and shuts the renderer down. This will close the application.
        /// </summary>
        public static void Shutdown()
        {
            if (Directory.Exists("F/"))
            {
                Directory.Delete("F/", true);
            }

            Application.Exit();
        }

    }
}
