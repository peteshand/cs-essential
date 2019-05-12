using System;
using System.Diagnostics;

public static class Trace
{
    public static void trace(string message)
    {
        // get call stack
        StackTrace stackTrace = new StackTrace();

        string className = stackTrace.GetFrame(1).GetMethod().DeclaringType.Name;

        // get calling method name
        Console.WriteLine(className + ": " + message);

        //Console.WriteLine(message);
    }
}
