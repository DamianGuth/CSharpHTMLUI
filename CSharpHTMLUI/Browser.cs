using IEFixLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
                System.Console.WriteLine("ASM:" + resourceName);

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
