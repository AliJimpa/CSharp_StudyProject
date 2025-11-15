using System;
using System.Threading;
using System.Runtime.InteropServices;

static class AJ_ConsoleWindow
{


    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();
    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;
    private static IntPtr consoleHandle = GetConsoleWindow();


    public static string ShowConsoleWindow(bool State)
    {
        if (State)
        {
            // Show the console window
            ShowWindow(consoleHandle, SW_SHOW);
            return "Show the console window";
        }
        else
        {   
            // Hide the console window
            ShowWindow(consoleHandle, SW_HIDE);
            return "Hide the console window";
        }
    }



}
