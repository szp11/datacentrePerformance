using System;
using System.Security.Principal;
using LamedalCore;
using LamedalCore.zPublicClass.Test;
using LamedalCore.zz;
using Microsoft.Win32;
using Xunit;
using Xunit.Abstractions;

namespace datacentrePerformance.Test.NewFeatures
{
    public class NewFeature_InstalledApplications : pcTest
    {
        private readonly LamedalCore_ _lamed = LamedalCore_.Instance; // system library

        public NewFeature_InstalledApplications(ITestOutputHelper debug = null) : base(debug) { }

        [Fact]
        public void Windows_Groups_Test()
        {
            var id = WindowsIdentity.GetCurrent();
            _Debug.WriteLine(id.Name);
            _Debug.WriteLine("Groups:");
            foreach (IdentityReference idGroup in id.Groups)
            {
                _Debug.WriteLine("- "+idGroup.Translate(typeof(NTAccount)).Value);
            }
        }

        public string FindInstalledApplications(out string header, out int count, out int total)
        {
            header = "Application, EstimatedSize, Publisher, InstallLocation";
            var result = "";
            var pcName = Environment.MachineName;
            var keyAddress1 = @"SOFTWARE\MICROSOFT\WINDOWS\CURRENTVERSION\Uninstall";
            var keyAddress2 = @"SOFTWARE\MICROSOFT\WINDOWS\CURRENTVERSION";
            RegistryKey reg32 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pcName, RegistryView.Registry32);
            RegistryKey Reg64 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pcName, RegistryView.Registry64);

            RegistryKey key32 = reg32.OpenSubKey(keyAddress1);
            var key64 = Reg64.OpenSubKey(keyAddress1);

            count = 0;
            total = 0;
            result += Application_GetValue(key32, ref count, ref total);
            result += Application_GetValue(key64, ref count, ref total);

            return result;
        }

        private static string Application_GetValue(RegistryKey key32, ref int count, ref int total)
        {
            string result = "";
            foreach (string keyName in key32.GetSubKeyNames())
            {
                var subKey = key32.OpenSubKey(keyName);
                if (subKey.ValueCount > 2)
                {
                    var value = subKey.GetValue("DisplayName");
                    if (value != null)
                    {
                        var size = subKey.GetValue("EstimatedSize").zObject().AsInt();
                        var publisher = "" + subKey.GetValue("Publisher");
                        var installLocation = "" + subKey.GetValue("InstallLocation");
                        result += value + "," + size / 1024 + "MB," + publisher + "," + installLocation.NL();
                        count++;
                        total = total + size;
                    }
                }
            }

            return result;
        }

        [Fact]
        public void FindInstalledApplications_Test()
        {
            var apps = FindInstalledApplications(out var header, out var count, out var total);
            _Debug.WriteLine("Total Applications: " + count);
            _Debug.WriteLine("Total size: " + total / 1024 /1024 + "Gig");
            _Debug.WriteLine("--------------------------------------------------------------------");
            _Debug.WriteLine(header);
            _Debug.WriteLine(apps);
        }
    }
}