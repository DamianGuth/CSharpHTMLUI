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
            Renderer.SetText("TESTBUTTON", "CoolText");
            Renderer.SetText("coolDemo", Generic.GetRandomString(32));
            Renderer.GenerateElement("p", "TEXT", "body");
            Renderer.LoadCachedPage("indexKopie.html");
        }
    }
}
