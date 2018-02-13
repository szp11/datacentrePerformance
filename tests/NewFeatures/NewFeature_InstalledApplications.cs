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
        public void PC_andUserName()
        {
            _Debug.WriteLine(Environment.MachineName);
            _Debug.WriteLine(Environment.UserName);
            _Debug.WriteLine(Environment.OSVersion.ToString());

            var id = WindowsIdentity.GetCurrent();
            _Debug.WriteLine(id.Name);
            _Debug.WriteLine("Groups:");
            foreach (IdentityReference idGroup in id.Groups)
            {
                _Debug.WriteLine("- "+idGroup.Translate(typeof(NTAccount)).Value);
            }
        }

        [Fact]
        public void FindInstalledApplications()
        {
            var pcName = Environment.MachineName;
            var keyAddress1 = @"SOFTWARE\MICROSOFT\WINDOWS\CURRENTVERSION\Uninstall";
            var keyAddress2 = @"SOFTWARE\MICROSOFT\WINDOWS\CURRENTVERSION";
            RegistryKey reg32 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pcName, RegistryView.Registry32);
            RegistryKey reg64 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pcName, RegistryView.Registry64);

            RegistryKey key32 = reg32.OpenSubKey(keyAddress1);
            //var key64 = Reg64.OpenSubKey(key1);

            int ii = 0;
            int total = 0;
            foreach (string keyName in key32.GetSubKeyNames())
            {
                var subKey = key32.OpenSubKey(keyName);
                if (subKey.ValueCount > 2)
                {
                    var value = subKey.GetValue("DisplayName");
                    if (value != null)
                    {
                        var size = subKey.GetValue("EstimatedSize").zObject().AsInt();
                        _Debug.WriteLine(value + " -> " + size / 1024 + "MB");
                        ii++;
                        total = total + size;
                    }
                }
            }
            _Debug.WriteLine("-------------------------------");
            _Debug.WriteLine("Total Applications: " + ii);
            _Debug.WriteLine("Total size: " + total/1024+"MB");
        }
    }
}