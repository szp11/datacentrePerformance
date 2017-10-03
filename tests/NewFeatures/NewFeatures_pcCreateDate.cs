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

        [Fact]
        public void Manufacturer_Test()
        {
            // create management class object
            var mc = new ManagementClass("Win32_ComputerSystem");
            //collection to store all management objects
            ManagementObjectCollection moc = mc.GetInstances();
            if (moc.Count != 0)
            {
                foreach (ManagementObject mo in moc)
                {
                    // display general system information
                    _Debug.WriteLine("Model: {0}", mo["Model"].ToString());
                    _Debug.WriteLine("Machine Make: {0}", mo["Manufacturer"].ToString());
                    _Debug.WriteLine("Domain: {0}", mo["Domain"].ToString());
                    _Debug.WriteLine("TotalPhysicalMemory: {0}", mo["TotalPhysicalMemory"].ToString());
                }
            }

            var mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject mo in mos.Get())
            {
                _Debug.WriteLine("Processor: {0}", mo["Name"].ToString());
            }

            var bios = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            foreach (ManagementObject bios1 in bios.Get())
            {
                _Debug.WriteLine("Bios: {0}", bios1["Version"].ToString());
                //string[] BIOSVersions = (string[])bios1["BIOSVersion"];
                string dateStr = bios1["ReleaseDate"].ToString();   // 20161019000000.000000+000
                dateStr = dateStr.zvar_Id(".");  // 20161019000000
                string formatString = "yyyyMMddHHmmss";
                DateTime dt = DateTime.ParseExact(dateStr, formatString, null);
                _Debug.WriteLine("InstallDate: {0}".zFormat(dt));
                _Debug.WriteLine("InstallDate: {0}".zFormat(_lamed.Types.DateTime.Age_2Str(dt)));
            }

            //wait for user action
            //Console.ReadLine();
        }

        [Fact]
        public void GetWindowsInstallationDateTime()
        {
            var computerName = Environment.MachineName;
            //Microsoft.Win32.RegistryKey key = Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, computerName);
            var key = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, computerName, RegistryView.Registry64);
            key = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
            if (key != null)
            {
                DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0);
                Int64 regVal = Convert.ToInt64(key.GetValue("InstallDate").ToString());

                DateTime installDate = startDate.AddSeconds(regVal);

                _Debug.WriteLine(installDate.ToString());
            }

            //return DateTime.MinValue;
        }

        [Fact]
        public void OperatingSystem_Test()
        {
            _Debug.WriteLine($"Process memory: {_lamed.lib.System.activeProcessMemory()}");
            _Debug.WriteLine($"OS Platform: {_lamed.lib.System.OS_Platform()}");
            _Debug.WriteLine($"OS Version: {_lamed.lib.System.OS_Version()}");
            _Debug.WriteLine($"-----------------------------------");
            _Debug.WriteLine($"{_lamed.lib.System.OS_InfoAsStr()}");
            _Debug.WriteLine($"-----------------------------------");
            _Debug.WriteLine($"{_lamed.lib.System.OS_InfoAsStr(false)}");

            DataTable table1 = OS_InfoAsTable(_lamed.lib.System.OS_InfoAsStr());
            DataTable table2 = OS_InfoAsTable(_lamed.lib.System.OS_InfoAsStr(false));
            Assert.Equal(table1, table2);
            Assert.True(table1.Rows.Count > 0);
        }

        /// <summary>
        /// Parse the input string into a datatable
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public DataTable OS_InfoAsTable(string inputStr)
        {
            return new DataTable();
        }

    }
}