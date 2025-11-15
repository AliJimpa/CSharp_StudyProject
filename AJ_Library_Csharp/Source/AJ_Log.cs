using AJ_Text;

namespace AJ_Log
{

    public delegate void MessageWrite(string message, Logtype type);


    public enum Logtype
    {
        Information,
        Warning,
        Error,
        SystemError,
        Console,
        Core,
    }


    public static class Log
    {
        public static MessageWrite? LogMessage;
        public static Textfile LogFile = new Textfile("Log");
        private static string _username = Environment.UserName;

        public static void Print(string Message, Logtype Type)
        {
            LogFile.Write($"[{DateTime.Now.ToString()}] [{_username}] {Type}:{Message}");
            LogMessage!(Message, Type);
        }

        public static void Print(string Message)
        {
            LogFile.Write($"[{DateTime.Now.ToString()}] [{_username}] {Logtype.Information}:{Message}");
            LogMessage!(Message, Logtype.Information);
        }

        public static void Print(string Message, Logtype Type , bool Write)
        {
            if(Write)
                LogFile.Write($"[{DateTime.Now.ToString()}] [{_username}] {Type}:{Message}");

            LogMessage!(Message, Type);
        }

        public static void PrintC(string Message)
        {
            LogMessage!(Message, Logtype.Console);
        }

    }



}