using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

[ComVisible(true)]
public class Logger
{
    public void Log(string s)
    {
        Console.WriteLine(s);
    }
}