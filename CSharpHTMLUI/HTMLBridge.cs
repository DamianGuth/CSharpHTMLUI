using CSharpHTMLUI;
using System;
using System.Runtime.InteropServices;

[ComVisible(true)]
public class Logger
{
    public void Send(string s)
    {
        Console.WriteLine(s);
        HTMLEventHandler.HandleEvent(s);
    }
}