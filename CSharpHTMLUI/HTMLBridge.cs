using CSharpHTMLUI;
using System;
using System.Runtime.InteropServices;

[ComVisible(true)]
public class HTMLBridge
{
    public void Send(string s)
    {
        Logger.Log(s, LogLevel.INFO);
        HTMLEventHandler.HandleEvent(s);
    }
}