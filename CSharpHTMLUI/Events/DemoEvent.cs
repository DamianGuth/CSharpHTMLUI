using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Form1.webBrowser.Document.InvokeScript("eval", new Object[] { "(function() { document.getElementById(\"" + "body" + "\").style.backgroundColor = \"blue\" })()" });
            Browser.SetText("TESTBUTTON", "CoolText");
        }
    }
}
