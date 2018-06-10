using System;

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
            Form1.webBrowser.Document.InvokeScript("eval", new Object[] { "(function() { document.getElementById(\"" + "body" + "\").style.backgroundColor = \"blue\" })()" });
            Browser.SetText("TESTBUTTON", "CoolText");
            Browser.SetText("coolDemo", Generic.GetRandomString(32));
            Browser.GenerateElement("p", "TEXT", "body");
            Browser.LoadCachedPage("indexKopie.html");
        }
    }
}
