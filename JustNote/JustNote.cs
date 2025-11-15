using System;
using System.IO;
using AJ_Text;


namespace JustNote
{

    class Program
    {
        public static void Main()
        {
            bool power = true;
            bool autocleaner = false;
            Textfile mytext = new Textfile("MyNote");
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory) + "\\Backup");

            while (power)
            {
                Console.Write("\n>>");
                string? TextLine = Console.ReadLine();
                switch (TextLine)
                {
                    case "exit":
                        power = false;
                        break;
                    case "build":
                        string? newpath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\JustNote.txt";
                        Console.WriteLine(newpath);
                        using (StreamWriter writer = new StreamWriter(newpath, false))
                        {
                            writer.WriteLine(mytext.Read());
                        }
                        bool findnumber = true;
                        int backupnumber = 1;
                        while (findnumber)
                        {
                            if (File.Exists($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory)}Backup\\Backup{backupnumber}.txt"))
                            {
                                backupnumber++;
                            }
                            else
                            {
                                findnumber = false;
                            }
                        }
                        using (StreamWriter writer = new StreamWriter($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory)}Backup\\Backup{backupnumber}.txt"))
                        {
                            writer.WriteLine(mytext.Read());
                        }
                        mytext.ResetFile();
                        break;
                    case "read":
                        Console.WriteLine("///////////////////////////////////////////////////////////");
                        Console.Write(mytext.Read());
                        break;
                    case "cls":
                        Console.Clear();
                        break;
                    case "help":
                        Console.WriteLine("[read] : read current text file");
                        Console.WriteLine("[build] : save text file in Desctop & one copy in backup folder");
                        Console.WriteLine("[`f] : Auto cleaner Off");
                        Console.WriteLine("[`o] : Auto cleander ON");
                        Console.WriteLine("[cls] : clear the app screen");
                        Console.WriteLine("[exit] : close the app");
                        break;
                    case "`o":
                        autocleaner = true;
                        break;
                    case "`f":
                        autocleaner = false;
                        break;
                    default:
                        if (TextLine != null)
                        {
                            mytext.Write(TextLine);
                            if (autocleaner)
                                Console.Clear();
                        }
                        break;
                }
            }



        }
    }


}
