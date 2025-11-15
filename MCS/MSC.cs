using System;
using System.IO;
using System.Diagnostics;
using AJ_Json;
using AJ_Backup;
using AJ_CoreSystem;
using AJ_Log;
using System.Threading;


namespace MSC
{
    class Base
    {
        static string backupfolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup");
        static Jsonfile databank = new Jsonfile("SaveFile");
        static BackupSystem backup = new BackupSystem();
        static SaveF file = new SaveF();
        static int delaytime = 0;
        static Thread backgroundThread = new Thread(AutoBackup);



        public static void Main(string[] args)
        {
            // Define Argomans
            List<Argoman> MyArgomans = new List<Argoman>();
            CoreSystem.AddArgList("add", typeof(Base), "Add", "add new adress for backup", 1);
            CoreSystem.AddArgList("remove", typeof(Base), "Remove", "Remove a address", 1);
            CoreSystem.AddArgList("backup", typeof(Base), "Backup", "Backup Data", 0);
            CoreSystem.AddOption("timer", typeof(Base), "Timer", "Automatic Backup", 1, 1);
            CoreSystem.AddOption("off", typeof(Base), "AutoOff", "Turn of automatic Backup", 1, 0);
            CoreSystem.AddArgList("list", typeof(MSC.Base), "List", "make list of all address", 0);
            CoreSystem.AddArgList("history", typeof(Base), "History", "Make List of All Backup History", 0);
            CoreSystem.AddArgList("open", typeof(Base), "Open", "Open a index Backup", 1);

            // Define Engine Setting
            CoreEngine.CoreSetting setting = new CoreEngine.CoreSetting();
            setting.StartText = "--------Backup Application---------";

            // Implement Delegates
            backup._backupLog += BackupLog;
            Log.LogMessage += LogSystem;

            // Backup folder
            if (!Directory.Exists(backupfolder))
            {
                Directory.CreateDirectory(backupfolder);
                SaveFile();
            }

            // Implement CoreEngine
            CoreEngine Engine = new CoreEngine(CoreSystem.GetArgList());
            Engine.NewSetting(setting);
            Engine.Run(args);
            CoreSystem.CleanList();

        }

        public static void Add(string Address)
        {
            if (Directory.Exists(Address))
            {
                LoadFile();
                file.AddNewPath(Address);
                Log.Print("Newpath Added");
                SaveFile();
            }else{
                Log.Print("Could not find a part of the path" , Logtype.Error);
            }            
        }
        public static void Remove(int Index)
        {
            try
            {
                LoadFile();
                file.RemoveNewPath(Index);
                Log.Print($"Removed [{Index}]");
                SaveFile();
            }
            catch (Exception e)
            {
                Log.Print(e.Message, Logtype.SystemError);
                throw;
            }
        }
        public static void List()
        {
            LoadFile();
            if (file.paths.Count > 0)
            {
                for (int i = 0; i < file.paths.Count; i++)
                {
                    Console.WriteLine($"{i}. {new DirectoryInfo(file.paths[i]).Name}  : {file.paths[i]}");
                }
            }
            else
            {
                Log.Print("Empty", Logtype.Information);
            }
        }
        public static void Backup()
        {
            LoadFile();
            foreach (string item in file.paths!)
            {
                try
                {
                    string BD = backup.BackupDirectory(item, backupfolder);
                    file.NewBackup(item, BD);
                }
                catch (Exception e)
                {
                    Log.Print(e.Message, Logtype.SystemError);
                    throw;
                }

            }
            SaveFile();
        }
        public static void History()
        {
            LoadFile();
            if (file.backuplist.Count > 0)
            {
                for (int i = 0; i < file.backuplist.Count; i++)
                {
                    SaveF.Backupdetail BD = file.backuplist[i];
                    string foldername = new DirectoryInfo(BD.backuppath).Name;
                    Console.WriteLine($"{i}. {foldername} : {BD.path}  [{BD.backuptime}]");
                }
                SaveFile();
            }
            else
            {
                Log.Print("Empty", Logtype.Warning);
            }
        }
        public static void Open(int Index)
        {
            LoadFile();
            int _index = Index;
            try
            {
                Console.WriteLine($"Open : [{file.backuplist[_index].backuppath}]");
                Process.Start("explorer.exe", file.backuplist[_index].backuppath);
            }
            catch (Exception e)
            {
                Log.Print(e.Message, Logtype.SystemError);
                throw;
            }
        }
        public static void AutoOff()
        {
            backgroundThread.Join();
        }
        public static void Timer(int time)
        {
            if (time > 0.1)
            {
                delaytime = time * 60* 1000;
                backgroundThread.IsBackground = true;
                backgroundThread.Start();
            }
            else
            {
                Log.Print("your time have to grater than 300,000", Logtype.Error);
            }

        }





        static void AutoBackup()
        {
            while (true)
            {
                Log.Print("Auto Backup-------------------");
                Backup();
                Thread.Sleep(delaytime);
            }
        }



        public static void BackupLog(string message)
        {
            Log.Print(message, Logtype.Console);
        }
        public static void LogSystem(string message, Logtype type)
        {
            if (type == Logtype.Console)
            {
                Console.WriteLine(message);
            }
            else
            {
                if (type != Logtype.Core)
                    Console.WriteLine($"{type}: {message}");
            }
        }


        public static void SaveFile()
        {
            databank.Write<SaveF>(file);
            Log.Print("File Saved", Logtype.Core);
        }
        public static void LoadFile()
        {
            file = databank.Read<SaveF>()!;
            Log.Print("File Loaded", Logtype.Core);
        }


    }





    public class SaveF
    {
        public struct Backupdetail
        {
            public string foldername;
            public string path;
            public string backuppath;
            public DateTime backuptime;
        }


        public List<string> paths = new List<string>();
        public List<Backupdetail> backuplist = new List<Backupdetail>();

        public void AddNewPath(string newpath)
        {
            paths.Add(newpath);
        }

        public void RemoveNewPath(int reindex)
        {
            paths.RemoveAt(reindex);
        }

        public void NewBackup(string newpath, string backupDestination)
        {
            Backupdetail newbd;
            newbd.foldername = new DirectoryInfo(newpath).Name;
            newbd.path = newpath;
            newbd.backuppath = backupDestination;
            newbd.backuptime = DateTime.Now;
            backuplist.Add(newbd);
        }



    }




}