using System.Reflection;


namespace AJ_CoreSystem
{
    public struct Option
    {
        private string Oname = "_Null_";
        private MethodInfo? OMethod;
        private string Odiscription = "";
        private int Oindex = -100;
        private string[] Oinputs;
        private bool validation = false;
        private string Odipencyname = "";

        public Option(string Name, string Discription, MethodInfo MethodInfo, int Index, int Input)
        {
            if (Name == "")
                Console.WriteLine("Error");

            Oname = Name;
            OMethod = MethodInfo;
            Odiscription = Discription;
            Oindex = Index;
            Oinputs = new string[Input];
            Odipencyname = "_0_";
            validation = true;
        }
        public Option(string Name, string Discription, MethodInfo MethodInfo, int Index, int Input, string dipency)
        {
            if (Name == "")
                Console.WriteLine("Error");

            Oname = Name;
            OMethod = MethodInfo;
            Odiscription = Discription;
            Oindex = Index;
            Oinputs = new string[Input];
            Odipencyname = dipency;
            validation = true;
        }
        public int GetIndex()
        {
            return Oindex;
        }
        public string GetName()
        {
            return Oname;
        }
        public string GetDiscription()
        {
            return Odiscription;
        }
        public void InvokeMethod(bool withparametr)
        {
            try
            {
                if (withparametr)
                {
                    OMethod!.Invoke(null, GetParametr());
                }
                else
                {
                    OMethod!.Invoke(OMethod.Name, null );
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public int GetInput()
        {
            if (Oinputs != null)
            {
                return Oinputs.Length;
            }
            else
            {
                return -1;
            }

        }
        public MethodInfo? GetMethodInfo()
        {
            return OMethod;
        }
        public void AddParametr(int index, string newparametr)
        {
            Oinputs[index] = newparametr;
        }
        public object[] GetParametr()
        {
            try
            {
                if (OMethod != null)
                {
                    // Get the parameters of the method
                    ParameterInfo[] parameters = OMethod.GetParameters();
                    // Create an array to store the arguments
                    object[] arguments = new object[parameters.Length];
                    // Prompt the user to enter values for each parameter
                    for (int i = 0; i < Oinputs.Length; i++)
                    {
                        arguments[i] = Convert.ChangeType(Oinputs[i], parameters[i].ParameterType);
                    }
                    return arguments;
                }
                else
                {
                    Console.WriteLine("Error");
                    return new object[1];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new object[1];
            }

        }
        public bool IsValid()
        {
            return validation;
        }
        public bool IsDipency(Option opt)
        {
            if (opt.GetName() == Odipencyname)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool HasDipency()
        {
            if (Odipencyname == "_0_")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public string GetDipency()
        {
            return Odipencyname;
        }
        public void InvokeMethod()
        {
            // Get the parameters of the method
            ParameterInfo[] parameters = OMethod!.GetParameters();

            // Create an array to store the arguments
            object[] arguments = new object[parameters.Length];

            // Prompt the user to enter values for each parameter
            for (int i = 0; i < parameters.Length; i++)
            {
                Console.WriteLine("Enter the value for parameter '{0}':", parameters[i].Name);
                string input = Console.ReadLine()!;
                arguments[i] = Convert.ChangeType(input, parameters[i].ParameterType);
            }

            // Invoke the method dynamically with the arguments
            OMethod.Invoke(null, arguments);
        }
    }

    class Argoman
    {
        private string? Argname;
        private List<Option> Options = new List<Option>();

        public Argoman(string _name, string _discription)
        {
            Argname = _name;
            MethodInfo? JustClas = default;
            Option newoption = new Option("OL#@", _discription, JustClas!, 0, 0);
            Options.Add(newoption);
        }
        public Argoman(string _name, Type _class, string _method, string _discription, int _inputnumber)
        {
            Argname = _name;
            AddOption(_name, _class, _method, _discription, 0, _inputnumber);
        }

        public void AddOption(string _name, Type _class, string _method, string _discription, int _index, int _inputnumber)
        {
            MethodInfo? MI = _class.GetMethod(_method, BindingFlags.Public | BindingFlags.Static);
            Option newoption = new Option(_name, _discription, MI!, _index, _inputnumber);
            Options.Add(newoption);
        }
        public void AddOption(string _name, Type _class, string _method, string _discription, int _index, int _inputnumber, string dipency)
        {
            MethodInfo? MI = _class.GetMethod(_method, BindingFlags.Public | BindingFlags.Static);
            Option newoption = new Option(_name, _discription, MI!, _index, _inputnumber, dipency);
            Options.Add(newoption);
        }

        public List<Option> GetAllOptions()
        {
            List<Option> NewList = Options;
            return NewList;
        }
        public string GetName()
        {
            return Argname!;
        }
        public Option GetSelf()
        {
            return Options[0];
        }
        public List<Option> GetOptions(int index)
        {
            List<Option> result = new List<Option>();
            foreach (var item in Options)
            {
                if (item.GetIndex() == index)
                {
                    result.Add(item);
                }
            }
            return result;
        }

    }


    class CoreEngine
    {
        public struct CoreSetting
        {
            public bool DynamicEngine;
            public bool CoreLoop;
            public string StartText;
            public string Cursor;

            public CoreSetting()
            {
                DynamicEngine = true;
                CoreLoop = false;
                StartText = "---------------------------------";
                Cursor = ">> ";
            }
        }


        private List<Argoman> Argomans = new List<Argoman>();
        private CoreSetting Setting = new CoreSetting();

        public CoreEngine(List<Argoman> argomanlist)
        {
            Argomans = argomanlist;

            //Add Help Argoman
            Argoman Help = new Argoman("help", "Print Discription for all Argoman"); Argomans.Add(Help);
            Argoman Exit = new Argoman("exit", "Close Concole App"); Argomans.Add(Exit);
            Argoman CleanConsole = new Argoman("cls", "Clear Console"); Argomans.Add(CleanConsole);

        }

        public void Run(string[] _args)
        {

            if (_args.Length == 0)
            {
                Setting.CoreLoop = true;
                Console.WriteLine(Setting.StartText);
            }
            else
            {
                Engine(_args);
            }

            while (Setting.CoreLoop)
            {
                Console.Write(Setting.Cursor);
                Engine(Console.ReadLine()!.Split(' '));
            }


        }


        public void NewSetting(CoreSetting NewSetting)
        {
            Setting = NewSetting;
        }


        public void Engine(string[] _argomans)
        {
            string[] args = _argomans;
            bool findarg = false;
            foreach (var arg in Argomans)
            {
                if (args[0] == arg.GetName())
                {
                    findarg = true;
                    if (arg.GetSelf().GetInput() > 0 && arg.GetSelf().GetInput() + 1 == args.Length)
                    {
                        for (int i = 0; i < args.Length; i++)
                        {
                            RunInputOption(arg.GetSelf(), ref i, args);
                        }
                    }
                    else
                    {
                        // Is There any Options
                        if (args.Length > 1)
                        {
                            AJ_CoreSystem.Option CurrentOption = new AJ_CoreSystem.Option();
                            int resetvalue = 0;
                            for (int i = 1; i < args.Length; i++)
                            {

                                bool IsFoundOption = false;
                                foreach (var opt in arg!.GetOptions(i - resetvalue))
                                {
                                    if (opt.GetName() == args[i])
                                    {
                                        IsFoundOption = true;

                                        // Detect Dipency Option
                                        if (opt.HasDipency())
                                        {
                                            if (!opt.IsDipency(CurrentOption))
                                            {
                                                Print(args[i], 3);
                                                break;
                                            }
                                        }

                                        // Detect Input Options
                                        if (opt.GetInput() > 0 && args.Length > i + opt.GetInput())
                                        {
                                            resetvalue += RunInputOption(opt, ref i, args);
                                        }
                                        else
                                        {
                                            // Is there any argoman
                                            if (args.Length == i + 1)
                                            {
                                                // Detect have input
                                                if (opt.GetInput() == 0)
                                                {
                                                    opt.InvokeMethod(false);
                                                }
                                                else
                                                {
                                                    if (Setting.DynamicEngine)
                                                    {
                                                        opt.InvokeMethod();
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine($"Need to Parametr for [{opt.GetName()}]");
                                                    }


                                                }

                                            }
                                            else
                                            {
                                                CurrentOption = opt;
                                            }
                                        }

                                    }
                                }

                                if (!IsFoundOption)
                                {
                                    Print(args[i], 2);
                                    break;
                                }
                            }

                        }
                        else
                        {
                            if (arg.GetSelf().GetName() != "OL#@" && arg.GetSelf().GetInput() == 0)
                            {
                                arg.GetSelf().InvokeMethod(false);
                            }
                            else
                            {
                                switch (arg.GetName())
                                {
                                    case "help":
                                        foreach (var item in Argomans)
                                        {
                                            HelpOptions(item);
                                        }
                                        break;
                                    case "exit":
                                        Setting.CoreLoop = false;
                                        break;
                                    case "cls":
                                        Console.Clear();
                                        break;
                                    default:
                                        arg.GetSelf().InvokeMethod();
                                        break;
                                }

                            }

                        }
                    }

                }

            }

            if (!findarg)
                Print(args[0], 1);
        }

        public int RunInputOption(AJ_CoreSystem.Option Option, ref int argidex, string[] args)
        {

            AJ_CoreSystem.Option targetOption = Option;
            for (int j = 0; j < targetOption.GetInput(); j++)
            {
                argidex++;
                targetOption.AddParametr(j, args[argidex]);
            }
            targetOption.InvokeMethod(true);
            return targetOption.GetInput() + 1;
        }

        public void Print(string message, int Code)
        {
            string type;
            switch (Code)
            {
                case 1:
                    type = "Argoman";
                    break;
                case 2:
                    type = "Option";
                    break;
                case 3:
                    type = "Option Depency";
                    break;
                default:
                    type = "Argoman";
                    break;
            }
            Console.WriteLine($"[{message}] : Wrong {type}");
        }

        public void HelpOptions(Argoman arg)
        {
            if (arg.GetAllOptions().Count > 1)
                Console.WriteLine($"---- {arg.GetName()} ----");
            foreach (var item in arg.GetAllOptions())
            {
                string MessageText = "";
                if (item.GetName() == "OL#@")
                {
                    MessageText = $"[{arg.GetName()}]";
                }
                else
                {
                    if (item.HasDipency())
                    {
                        MessageText = $"[{item.GetDipency()} {item.GetName()}]";
                    }
                    else
                    {
                        MessageText = $"[{item.GetName()}]";
                    }
                    for (int i = 0; i < item.GetInput(); i++)
                    {
                        MessageText += $" (Input_{i})";
                    }
                }

                if (item.GetIndex() != 0)
                {
                    for (int i = 0; i < item.GetIndex(); i++)
                    {
                        MessageText = "  " + MessageText;
                    }
                }


                Console.WriteLine(MessageText + $" : {item.GetDiscription()}");
            }
        }




    }


    static class CoreSystem
    {

        private static List<Argoman> BaseArgomans = new List<Argoman>();
        private static Argoman? BaseArg;

        public static void AddArgList(string _name, Type _class, string _method, string _discription, int _inputnumber)
        {
            if (BaseArg != null)
                BaseArgomans.Add(BaseArg);
            BaseArg = new Argoman(_name, _class, _method, _discription, _inputnumber);
        }
        public static void AddArgList(string _name, string _discription)
        {
            if (BaseArg != null)
                BaseArgomans.Add(BaseArg);
            BaseArg = new Argoman(_name, _discription);
        }
        public static void AddOption(string _name, Type _class, string _method, string _discription, int _index, int _inputnumber)
        {
            BaseArg!.AddOption(_name, _class, _method, _discription, _index, _inputnumber);
        }
        public static void AddOption(string _name, Type _class, string _method, string _discription, int _index, int _inputnumber, string dipency)
        {
            BaseArg!.AddOption(_name, _class, _method, _discription, _index, _inputnumber, dipency);
        }

        public static void CleanList()
        {
            BaseArgomans.Clear();
        }
        public static List<Argoman> GetArgList()
        {
            if (BaseArg != null)
                BaseArgomans.Add(BaseArg);
            return BaseArgomans;
        }

    }


}