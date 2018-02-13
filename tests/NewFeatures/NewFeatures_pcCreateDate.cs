using System;
using System.Data;
using System.Management;
using LamedalCore;
using LamedalCore.zPublicClass.Test;
using LamedalCore.zz;
using Microsoft.Win32;
using Xunit;
using Xunit.Abstractions;

namespace datacentrePerformance.Test.NewFeatures
{
    public class NewFeatures_pcCreateDate : pcTest
    {
        private readonly LamedalCore_ _lamed = LamedalCore_.Instance;

        public NewFeatures_pcCreateDate(ITestOutputHelper debug = null) : base(debug) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="format2CSV"></param>
        /// <returns></returns>
        public string BIOS_AsStr(out string header, bool format2CSV = false)
        {
            header = "";
            var newLine = format2CSV.zCSV_NewLine();
            var biModel = "Model".zCSV_FormatItem(ref header, format2CSV);
            var biManufacturer = "Manufacturer".zCSV_FormatItem(ref header, format2CSV);
            var biDomain = "Domain".zCSV_FormatItem(ref header, format2CSV);
            var biProcessor = "Processor".zCSV_FormatItem(ref header, format2CSV);
            var biBios = "Bios".zCSV_FormatItem(ref header, format2CSV);
            var biReleaseDate = "ReleaseDate".zCSV_FormatItem(ref header, format2CSV);
            var biAge = "ReleaseAge".zCSV_FormatItem(ref header, format2CSV);
            var biWindowsDate = "WindowsInstallDate".zCSV_FormatItem(ref header, format2CSV);
            var biWindowsAge = "WindowsAge".zCSV_FormatItem(ref header, format2CSV);

            var result = "";
            var mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            if (moc.Count != 0)
            {
                foreach (ManagementObject mo in moc)
                {
                    result += $"{biModel}{mo["Model"]}{newLine}";
                    result += $"{biManufacturer}{mo["Manufacturer"]}{newLine}";
                    result += $"{biDomain}{mo["Domain"]}{newLine}";
                }

                var mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject mo in mos.Get())
                {
                    result += $"{biProcessor}{mo["Name"]}{newLine}";
                }

                var bios = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                foreach (ManagementObject bios1 in bios.Get())
                {
                    result += $"{biBios}{bios1["Version"]}{newLine}";

                    string dateStr = bios1["ReleaseDate"].ToString();   // 20161019000000.000000+000
                    dateStr = ".".zVar_Id(dateStr);  // 20161019000000
                    string formatString = "yyyyMMddHHmmss";
                    DateTime dt = DateTime.ParseExact(dateStr, formatString, null);
                    result += $"{biReleaseDate}{dt.zTo_Str()}{newLine}";
                    if (format2CSV == false) result += $"{biAge}{dt.zAge_2Str()}{newLine}";
                }
            }
            result += $"{biWindowsDate}{Windows_InstallationDate().zTo_Str()}{newLine}";
            if (format2CSV == false) result += $"{biWindowsAge}{Windows_InstallationDate().zAge_2Str()}{newLine}";

            return result;
        }

        [Fact]
        public void Manufacturer_Test1()
        {
            _Debug.WriteLine(BIOS_AsStr(out var header).NL());
            var line =_lamed.lib.System.Runtime.OS_ID_AsStr(out header, true) + BIOS_AsStr(out var header2, true);
            header += header2;
            _Debug.WriteLine(header);
            _Debug.WriteLine(line);
        }

        public DateTime Windows_InstallationDate()
        {
            var computerName = Environment.MachineName;
            //Microsoft.Win32.RegistryKey key = Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, computerName);
            var key = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, computerName, RegistryView.Registry64);
            key = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
            if (key != null)
            {
                DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0);
                Int64 regVal = Convert.ToInt64(key.GetValue("InstallDate").ToString());

                var installDate = startDate.AddSeconds(regVal);
                return installDate;
                //_Debug.WriteLine("Windows install date: " +installDate.zTo_Str());
            }

            return DateTime.MinValue;
        }

        [Fact]
        public void OperatingSystem_Test()
        {
            _Debug.WriteLine(_lamed.lib.System.Runtime.OS_ID_AsStr(out var header));
            _Debug.WriteLine(header);
            _Debug.WriteLine(_lamed.lib.System.Runtime.OS_ID_AsStr(out header, true));
            _Debug.WriteLine($"-----------------------------------".NL());
            _Debug.WriteLine($"{_lamed.lib.System.Runtime.OS_Details_AsStr(out header)}");
            _Debug.WriteLine(header);
            _Debug.WriteLine($"{_lamed.lib.System.Runtime.OS_Details_AsStr(out header, true)}");

            _Debug.WriteLine($"-------------------------------------------".NL());
            _Debug.WriteLine($"Process memory: {_lamed.lib.System.Runtime.activeProcessMemory()}");

        }

    }
}