using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpHTMLUI.Events
{
    public interface EventTemplate
    {
        string GetId();
        void OnClick();
    }
}
