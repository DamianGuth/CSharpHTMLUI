using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpHTMLUI.Events
{
    public class CalculateBtnEvent : EventTemplate
    {
        string id = "calculate-btn";

        public string GetId()
        {
            return id;
        }

        public void OnClick()
        {
            int result = 0;
            int firstBox = 0;
            int scndBox = 0;

            int.TryParse(Browser.GetElementText("firstBox"), out firstBox);
            int.TryParse(Browser.GetElementText("secondBox"), out scndBox);

            try
            {
                result = firstBox + scndBox;
                Browser.SetText("resultText", result.ToString());
            }
            catch (Exception ex)
            {
            }
        }
    }
}
