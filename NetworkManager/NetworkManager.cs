using System;
using System.IO;
using System.Diagnostics;
using AJ_CoreSystem;
using AJ_Json;
using AJ_Log;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.Versioning;

namespace NetworkManager
{
    class Base
    {
        static Jsonfile databank = new Jsonfile("SaveFile");
        static SaveF file = new SaveF();

        public static void Main(string[] args)
        {
            // Define Argomans
            CoreSystem.AddArgList("check", typeof(Base), "Check", "Check System DNS", 0);
            CoreSystem.AddArgList("add", typeof(Base), "Add", "Add DNS code to list", 2);
            CoreSystem.AddArgList("remove", typeof(Base), "Remove", "remove that saved Dns befor", 1);
            CoreSystem.AddArgList("list", typeof(Base), "List", "Make List of all your dns that saved them", 0);
            CoreSystem.AddArgList("dns", typeof(Base), "ConfigDNS", "Set New DNs for Network", 1);

            // Define Engine Setting
            CoreEngine.CoreSetting setting = new CoreEngine.CoreSetting();
            setting.StartText = "--------NetworkManagement Application---------";

            //SaveFile();
            // Implement Delegates
            Log.LogMessage += LogSystem;

            // Implement CoreEngine
            CoreEngine Engine = new CoreEngine(CoreSystem.GetArgList());
            Engine.NewSetting(setting);
            Engine.Run(args);
            CoreSystem.CleanList();
        }

        public static void Check()
        {
            try
            {
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in adapters)
                {

                    IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                    IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
                    if (dnsServers.Count > 0)
                    {
                        Console.WriteLine(adapter.Description);
                        foreach (var dns in dnsServers)
                        {
                            Console.WriteLine("  DNS Servers ............................. : {0}", dns.ToString());
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Print($"An error occurred: {ex.Message}");
            }
        }

        public static void Add(String Primerycode, string SeconderycodeS)
        {
            LoadFile();
            file.AddDNS(Primerycode, SeconderycodeS);
            Log.Print("DNS Code Added");
            SaveFile();
        }

        public static void Remove(int index)
        {
            LoadFile();
            file.RemoveDNS(index);
            Log.Print("DNS Code Removed");
            SaveFile();
        }

        public static void List()
        {
            LoadFile();
            if (file.DNSList.Count > 0)
            {
                for (int i = 0; i < file.DNSList.Count; i++)
                {
                    Console.WriteLine($"{i}. {file.DNSList[i].InterfaceName} -- {file.DNSList[i].Type.ToString()} --> [{file.DNSList[i].primery}]:[{file.DNSList[i].Secondery}] ");
                }
            }
            else
            {
                Log.Print("Empty", Logtype.Information);
            }
        }


        // [SupportedOSPlatform("windows")]
        // public static void SetDNS(int index)
        // {
        //     LoadFile();
        //     SaveF.DNSCode MyDNS;
        //     if (file.IsValidIndex(index))
        //     {
        //         MyDNS = file.DNSList[index];

        //         try
        //         {
        //             DisplayDnsAddresses(MyDNS.primery, MyDNS.Secondery);
        //             Log.Print("DNS settings set successfully.");
        //         }
        //         catch (Exception ex)
        //         {
        //             Log.Print($"An error occurred: {ex.Message}");
        //         }

        //     }


        // }

        // static void SetDnsServers(string interfaceName, string preferredDns, string alternateDns)
        // {
        //     Process process = new Process();
        //     process.StartInfo.FileName = "netsh";
        //     process.StartInfo.Arguments = $"interface ip set dns name=\"{interfaceName}\" static {preferredDns} primary";
        //     process.StartInfo.Verb = "runas"; // Run as administrator
        //     process.StartInfo.RedirectStandardOutput = true;
        //     process.StartInfo.UseShellExecute = false;
        //     process.StartInfo.CreateNoWindow = true;

        //     process.Start();
        //     process.WaitForExit();

        //     // Set alternate DNS server
        //     process.StartInfo.Arguments = $"interface ip add dns name=\"{interfaceName}\" addr={alternateDns} index=2";
        //     process.Start();

        //     process.WaitForExit();
        // }


        [SupportedOSPlatform("windows")]
        public static void ConfigDNS(int index)
        {
            LoadFile();
            SaveF.DNSCode MyDNS;
            if (file.IsValidIndex(index))
            {
                MyDNS = file.DNSList[index];

                try
                {
                    // Get the current network interface
                    var currentInterface = GetActiveEthernetOrWifiNetworkInterface();
                    if (currentInterface == null)
                    {
                        Console.WriteLine("No active network interface found.");
                        return;
                    }

                    // Get the current DNS setting
                    var currentDNS = currentInterface.GetIPProperties().DnsAddresses;
                    Console.WriteLine("Current DNS setting: {0}", string.Join(", ", currentDNS));

                    // Set the new DNS setting
                    var newDNS = new string[] { MyDNS.primery , MyDNS.Secondery };
                    Console.WriteLine("New DNS setting: {0}", string.Join(", ", newDNS));
                    SetDNSServerSearchOrder(currentInterface, newDNS);

                    // Save the changes
                    Console.WriteLine("Saving changes...");
                    Save(currentInterface);

                    // Check the new DNS setting
                    var updatedInterface = GetActiveEthernetOrWifiNetworkInterface();
                    var updatedDNS = updatedInterface.GetIPProperties().DnsAddresses;
                    Console.WriteLine("Updated DNS setting: {0}", string.Join(", ", updatedDNS));
                }
                catch (System.Exception)
                {

                    throw;
                }

            }


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




        // Get the active network interface
        public static NetworkInterface GetActiveEthernetOrWifiNetworkInterface()
        {
            var nic = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(
                a => a.OperationalStatus == OperationalStatus.Up &&
                (a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || a.NetworkInterfaceType == NetworkInterfaceType.Ethernet) &&
                a.GetIPProperties().GatewayAddresses.Any(g => g.Address.AddressFamily.ToString() == "InterNetwork"));

            return nic!;
        }

        // Set the DNS server search order
        [SupportedOSPlatform("windows")]
        public static void SetDNSServerSearchOrder(NetworkInterface nic, string[] dns)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    if (mo["Description"].ToString()!.Equals(nic.Description))
                    {
                        ManagementBaseObject objdns = mo.GetMethodParameters("SetDNSServerSearchOrder");
                        if (objdns != null)
                        {
                            objdns["DNSServerSearchOrder"] = dns;
                            mo.InvokeMethod("SetDNSServerSearchOrder", objdns, null);
                        }
                    }
                }
            }
        }

        // Save the changes
        [SupportedOSPlatform("windows")]
        public static void Save(NetworkInterface nic)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    if (mo["Description"].ToString()!.Equals(nic.Description))
                    {
                        mo.InvokeMethod("Save", null, null);
                    }
                }
            }
        }

    }







    public class SaveF
    {
        public enum DNSType
        {
            IPv4,
            ipv6,
        }

        public struct DNSCode
        {
            public string InterfaceName;
            public DNSType Type;
            public string primery;
            public string Secondery;
            public DateTime date;
        }

        public List<DNSCode> DNSList = new List<DNSCode>();

        public void AddDNS(string Primery, string Secondery)
        {
            DNSCode newdns;
            newdns.InterfaceName = "Ethernet";
            newdns.Type = DNSType.IPv4;
            newdns.primery = Primery;
            newdns.Secondery = Secondery;
            newdns.date = DateTime.Now;
            DNSList.Add(newdns);
        }

        public void RemoveDNS(int reindex)
        {
            DNSList.RemoveAt(reindex);
        }

        public bool IsValidIndex(int index)
        {
            return index >= 0 && index < DNSList.Count;
        }

    }



}
