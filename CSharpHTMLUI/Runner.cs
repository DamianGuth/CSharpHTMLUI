using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpHTMLUI
{
    public class Runner
    {
        public static Runner instance;
        private WebBrowser browserControl;
        private Control titleControl;
        private bool preventFix = false;

        public Runner(string defaultPage, Control newBrowserControl = null, bool preventEngineFix = false)
        {
            if (instance == null)
                instance = this;

            if (newBrowserControl != null && newBrowserControl.GetType() == typeof(WebBrowser))
                browserControl = (WebBrowser)newBrowserControl;

            preventFix = preventEngineFix;

            Renderer.defaultPage = defaultPage;

            browserControl.Navigated += new WebBrowserNavigatedEventHandler(navigated);
        }

        public void Initiate()
        {
            Renderer.Initialize(preventFix);
            HTMLEventHandler.AutoRegisterEvents();
        }

        public void SetTitleControl(Control newTitleControl)
        {
            titleControl = newTitleControl;
        }

        public string GetPageTitle()
        {
            return browserControl.Document.Title;
        }

        /// <summary>
        /// Closes the application and does some cleanup
        /// </summary>
        public static void Shutdown()
        {
            if (Directory.Exists("F/"))
            {
                Directory.Delete("F/", true);
            }
            Application.Exit();
        }

        internal void PlainNavigate(string url)
        {
            browserControl.Navigate(url);
        }

        internal void SetHTMLBridge(object bridge)
        {
            browserControl.ObjectForScripting = bridge;
        }

        internal void OpenPageClean(string pageData)
        {
            browserControl.DocumentText = "0";
            browserControl.Document.OpenNew(true);
            browserControl.Document.Write(pageData);
            browserControl.Refresh();
        }


        #region document manipulation

        public string GetDocumentText()
        {
            return browserControl.DocumentText;
        }

        public HtmlElementCollection GetElementsByTagName(string tag)
        {
            return browserControl.Document.GetElementsByTagName(tag);
        }

        public HtmlElement GetElementById(string id)
        {
            return browserControl.Document.GetElementById(id);
        }

        public HtmlElement CreateElement(string elementTag)
        {
            return browserControl.Document.CreateElement(elementTag);
        }

        public object ExecuteJavascript(string name, object[]args)
        {
            return browserControl.Document.InvokeScript(name, args);
        }

        public object ExecuteJavascript(string script)
        {
            return browserControl.Document.InvokeScript(script);
        }

        #endregion

        #region events

        private void navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if(titleControl != null)
            titleControl.Text = GetPageTitle();

            Renderer.InjectMethods();
        }

        #endregion
    }
}
