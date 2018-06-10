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
            long result = 0;
            long firstBox = 0;
            long scndBox = 0;

            long.TryParse(Browser.GetElementText("firstBox"), out firstBox);
            long.TryParse(Browser.GetElementText("secondBox"), out scndBox);

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
