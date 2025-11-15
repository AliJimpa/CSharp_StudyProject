using System;
using System.Diagnostics;
using AJ_Json;
using AJ_Text;

namespace PCCleaner
{
    class Program
    {
        public static void Main()
        {
            bool power = true;
            Debug debug = new Debug();
            Jsonfile SaveFile = new Jsonfile("DataBank");
            Pathdata NewPD = SaveFile.Read<Pathdata>() ?? new Pathdata();

            debug.LogSystem("StartApp", Debug.Logtype.info);
            while (power)
            {
                Console.Write("\n>>");
                string[]? args = Console.ReadLine().Split(' ');
                switch (args[0])
                {
                    case "add":
                        debug.LogSystem("Add new path address", Debug.Logtype.info);
                        try
                        {
                            NewPD.AddItem(args[1], args[2]);
                            SaveFile.Write<Pathdata>(NewPD);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            debug.LogSystem(e.Message, Debug.Logtype.erro);
                        }
                        break;
                    case "list":
                        debug.LogSystem("Make list of all path", Debug.Logtype.info);
                        NewPD = SaveFile.Read<Pathdata>() ?? NewPD;
                        int index = 0;
                        foreach (Pathdata.Apath item in NewPD.GetAllPath())
                        {
                            Console.WriteLine($"{index}.{item.identity}  [{item.path}]");
                            index++;
                        }
                        break;
                    case "clean":
                        debug.LogSystem("Clean path", Debug.Logtype.info);
                        switch (args[1])
                        {
                            case ".":
                                foreach (Pathdata.Apath item in NewPD.GetAllPath())
                                {
                                    if (Directory.Exists(item.path))
                                    {
                                        try
                                        {
                                            DirectoryInfo di = new DirectoryInfo(item.path);
                                            Console.WriteLine("Are you Sure? y/n");
                                            if (Console.ReadLine() == "y")
                                            {
                                                di.Delete(true);
                                                Console.WriteLine($"[{item.path}]  Deleted.  -*");
                                                debug.LogSystem($"[{item.path}]  Deleted.  -*", Debug.Logtype.erro);
                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                            debug.LogSystem(e.Message, Debug.Logtype.erro);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"[{item.path}]  Directory Does not exist!  -#");
                                        debug.LogSystem($"[{item.path}]  Directory Does not exist!  -#", Debug.Logtype.erro);

                                    }
                                }
                                break;
                            default:
                                foreach (Pathdata.Apath item in NewPD.GetAllPath())
                                {
                                    if (item.identity == args[1])
                                    {
                                        try
                                        {
                                            DirectoryInfo di = new DirectoryInfo(item.path);
                                            Console.WriteLine("Are you Sure? y/n");
                                            if (Console.ReadLine() == "y")
                                            {
                                                di.Delete(true);
                                                Console.WriteLine($"[{item.path}]  Deleted.  -*");
                                                debug.LogSystem($"[{item.path}]  Deleted.  -*", Debug.Logtype.erro);
                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                            debug.LogSystem(e.Message, Debug.Logtype.erro);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"[{item.path}]  Directory Does not exist!  -#");
                                        debug.LogSystem($"[{item.path}]  Directory Does not exist!  -#", Debug.Logtype.erro);
                                    }
                                }
                                break;

                        }
                        break;
                    case "remove":
                        debug.LogSystem("Remove path", Debug.Logtype.info);
                        NewPD = SaveFile.Read<Pathdata>() ?? NewPD;
                        try
                        {
                            NewPD.RemovePath(int.Parse(args[1]));
                            SaveFile.Write<Pathdata>(NewPD);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            debug.LogSystem(e.Message, Debug.Logtype.erro);
                        }
                        break;
                    case "open":
                        debug.LogSystem("Open path", Debug.Logtype.info);
                        NewPD = SaveFile.Read<Pathdata>() ?? NewPD;
                        foreach (Pathdata.Apath item in NewPD.GetAllPath())
                        {
                            if (item.identity == args[1])
                            {
                                try
                                {
                                    Process.Start("explorer.exe", item.path);
                                    Console.WriteLine(item.path);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    debug.LogSystem(e.Message, Debug.Logtype.erro);
                                }

                            }
                        }
                        break;
                    case "help":
                        Console.WriteLine("[add] [NameSlot] [Path Address] : For Ading new path slot");
                        Console.WriteLine("[list] : For showing list of all path");
                        Console.WriteLine("[clean] [NameSlot/all] : For Delete Specific folder / all slot with '.' ");
                        Console.WriteLine("[remove] [Index] : For removing a slot with SlotIndex");
                        Console.WriteLine("[open] [NameSlot] : For opening address folder");
                        Console.WriteLine("[exit] : For closing program");
                        Console.WriteLine("[fixpath] [SlotIndex] (NewPath) : For fixing path");
                        break;
                    case "exit":
                        power = false;
                        break;
                    case "":
                        break;
                    case "fixpath":
                        try
                        {
                            Pathdata.Apath NewApath = NewPD.allpath[int.Parse(args[1])];
                            Console.Write("Write New Path-->>");
                            NewApath.path = Console.ReadLine() ?? "";
                            NewPD.allpath[int.Parse(args[1])] = NewApath;
                            SaveFile.Write<Pathdata>(NewPD);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            debug.LogSystem(e.Message, Debug.Logtype.erro);
                        }
                        break;
                    default:
                        Console.WriteLine($"No arguments passed [{args[0]}]");
                        debug.LogSystem($"No arguments passed [{args[0]}]", Debug.Logtype.erro);
                        break;
                }
                debug.LogSystem("FinishCycle System", Debug.Logtype.info);
            }


        }
    }

    class Debug
    {

        public enum Logtype
        {
            info,
            warn,
            erro,
        }
        private Textfile Log = new Textfile("Log");

        public void LogSystem(string message, Logtype type)
        {
            string _username = Environment.UserName;
            string _time = DateTime.Now.ToString();
            Log.Write($"[{type}] {_time} {_username}: {message}");
        }

    }

    class Pathdata
    {
        public struct Apath
        {
            public string identity;
            public string path;

            public int size;
            public int lastsize;
        }

        private int length;
        public List<Apath> allpath = new List<Apath>();

        public void AddItem(string _identity, string _path)
        {
            Apath newpath = new Apath();
            newpath.identity = _identity;
            newpath.path = _path;
            newpath.size = 100;
            newpath.lastsize = 0;
            allpath.Add(newpath);
            length++;
        }

        public int Getlength()
        {
            return length;
        }

        public Apath GetData(int index)
        {
            return allpath[index];
        }

        public List<Apath> GetAllPath()
        {
            return allpath;
        }

        public void RemovePath(int _index)
        {
            try
            {
                allpath.RemoveAt(_index);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }



}











