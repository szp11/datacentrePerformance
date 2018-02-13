using System;
using System.Security.Principal;
using LamedalCore;
using LamedalCore.zPublicClass.Test;
using LamedalCore.zz;
using LamedalWindows;
using Microsoft.Win32;
using Xunit;
using Xunit.Abstractions;

namespace datacentrePerformance.Test.NewFeatures
{
    public class NewFeature_InstalledApplications : pcTest
    {
        private readonly LamedalCore_ _lamed = LamedalCore_.Instance; // system library

        public NewFeature_InstalledApplications(ITestOutputHelper debug = null) : base(debug) { }

        
    }
}