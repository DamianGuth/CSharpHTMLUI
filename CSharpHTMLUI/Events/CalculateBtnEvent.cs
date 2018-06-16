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

            long.TryParse(Renderer.GetElementText("firstBox"), out firstBox);
            long.TryParse(Renderer.GetElementText("secondBox"), out scndBox);

            try
            {
                result = firstBox + scndBox;
                Renderer.SetText("resultText", result.ToString());
            }
            catch (Exception ex)
            {
            }
        }
    }
}
