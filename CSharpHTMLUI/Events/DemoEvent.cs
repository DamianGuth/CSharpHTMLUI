using System;
using System.Windows.Forms;

namespace CSharpHTMLUI.Events
{
    public class DemoEvent : EventTemplate
    {
        string id = "TESTBUTTON";

        public string GetId()
        {
            return id;
        }

        public void OnClick()
        {
            //Form1.webBrowser.Document.InvokeScript("eval", new Object[] { "(function() { document.getElementById(\"" + "body" + "\").style.backgroundColor = \"blue\" })()" });
            //Runner.instance.ExecuteJavascript("(function() { document.getElementById(\"" + "body" + "\").style.backgroundColor = \"blue\" })()");
            Runner.instance.ExecuteJavascript("eval", new Object[] { "(function() { document.getElementById(\"" + "body" + "\").style.backgroundColor = \"blue\" })()" });
            Renderer.SetText("TESTBUTTON", "CoolText");
            Renderer.SetText("coolDemo", Generic.GetRandomString(32));
            Renderer.GenerateElement("p", "TEXT", "body");
           // MessageBox.Show("Event registered and fired!");
            //Renderer.LoadCachedPage("indexKopie.html");
        }
    }
}
