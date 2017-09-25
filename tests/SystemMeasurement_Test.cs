using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using datacentrePerformance.domain.Attributes;
using datacentrePerformance.domain.Enumerals;
using LamedalCore;
using LamedalCore.domain.Attributes;
using LamedalCore.domain.Enumerals;
using LamedalCore.zPublicClass.Test;
using LamedalCore.zz;
using LamedalCore_Templates;
using Lamedal_UIWinForms;
using Lamedal_UIWinForms.domain.Enumerals;
using Xunit;
using Xunit.Abstractions;

namespace datacentrePerformance.Test
{
    public partial class SystemMeasurement_Test : pcTest
    {
        private readonly LamedalCore_ _lamed = LamedalCore_.Instance; // system library
        private readonly Lamedal_WinForms _lamedWin = Lamedal_WinForms.Instance;  // Load the winforms lib

        public SystemMeasurement_Test(ITestOutputHelper debug = null) : base(debug){}

        [Fact]
        [Test_Method("SystemMeasurement.Memory()")]
        public void SystemMeasurement_Tests()
        {
            // This method will test all sytem measurements
            _Debug.WriteLine("Memory:");
            foreach (var enum1 in _lamed.Types.Enum.Enumvalues<enCounter_Memory>()) _Debug.WriteLine("  " + SystemMeasurement.Memory(enum1));  // Memory
            _Debug.WriteLine("IO:");
            var letters = _lamed.lib.IO.Drive.Letters();
            foreach (string drive in letters)
            {
                _Debug.WriteLine("  "+drive);
                foreach (var enum1 in _lamed.Types.Enum.Enumvalues<enCounter_IO>()) _Debug.WriteLine("  " + SystemMeasurement.IO(enum1, drive));  // Disk
                _Debug.WriteLine("  ------------------------");
            }
        }

        [Fact]
        [Test_Method("zAttribute_AsPerformanceMeasure()")]
        [Test_Method("SystemMeasurement.Memory()")]
        public void Memory_enCounter_Test()
        {
            // Input
            var enumInput = enCounter_Memory.PhysicalMemory_Available;
            
            // Method 1
            var performanceMeasure = enumInput.zAttribute_AsPerformanceMeasure();
            SystemMeasurement.Memory(performanceMeasure);
            _Debug.WriteLine(performanceMeasure.ToString());
            // Tests
            Assert.Equal("Memory", performanceMeasure.Name_Category);
            Assert.Equal("Available Physical memory", performanceMeasure.Name_Caption);
            Assert.Equal(1, performanceMeasure.Format_DecimalPlaces);
            Assert.True(performanceMeasure.Value_> 0);

            // Method 2
            _Debug.WriteLine(SystemMeasurement.Memory(enumInput).ToString());
        }

        [Fact]
        [Test_Method("SystemMeasurement.Memory()")]
        public void Memory_Test()
        {
            _Debug.WriteLine("Memory:");
            if (Environment.Is64BitProcess)
                 _Debug.WriteLine("  64-bit process");
            else _Debug.WriteLine("  32-bit process");

            // activeProcessMemory
            var activeProcessMemory = SystemMeasurement.Memory(enCounter_Memory.activeProcessMemory);
            Assert.True(activeProcessMemory.Value_ > 0);
            _Debug.WriteLine($"  {activeProcessMemory}");
            _Debug.WriteLine("-----------------------------");
            _Debug.WriteLine("  " + SystemMeasurement.Memory(enCounter_Memory.PhysicalMemory_Total).ToString());
            _Debug.WriteLine("  " + SystemMeasurement.Memory(enCounter_Memory.PhysicalMemory_Available).ToString());
            _Debug.WriteLine("  " + SystemMeasurement.Memory(enCounter_Memory.PhysicalMemory_InUse).ToString());
            _Debug.WriteLine("-----------------------------");

            _Debug.WriteLine("  " + SystemMeasurement.Memory(enCounter_Memory.VirtualMemory_Total).ToString());
            _Debug.WriteLine("  " + SystemMeasurement.Memory(enCounter_Memory.VirtualMemory_Available).ToString());
            _Debug.WriteLine("  " + SystemMeasurement.Memory(enCounter_Memory.VirtualMemory_InUse).ToString());
            _Debug.WriteLine("-----------------------------");

            // page file
            enumPerformanceMeasureAttribute pagingFileUsage = SystemMeasurement.Memory(enCounter_Memory.pagingFileUsage);
            _Debug.WriteLine($"  {pagingFileUsage}");

            // pagingFilePeak
            enumPerformanceMeasureAttribute pagingFilePeak = SystemMeasurement.Memory(enCounter_Memory.pagingFilePeak);
            _Debug.WriteLine($"  {pagingFilePeak}");
            _Debug.WriteLine("-----------------------------");

            // PhysicalMemory_Available
            enumPerformanceMeasureAttribute availableMemory = SystemMeasurement.Memory(enCounter_Memory.pagingFileUsage);
            Assert.Equal("Paging file usage", availableMemory.Name_Caption);
            Assert.Equal("%", availableMemory.Value_Unit);
            Assert.Equal("Paging File", availableMemory.Name_Category);
            Assert.Equal("% Usage", availableMemory.Name_Counter);
        }

        [Fact]
        public void IO_Test()
        {
            // Speed
            _Debug.WriteLine("Drive Speed:");
            _Debug.WriteLine("  " + SystemMeasurement.IO(enCounter_IO.Speed));
            _Debug.WriteLine("Method2:");
            _Debug.WriteLine($"  Drive c: {_lamed.lib.IO.Drive.Drive_Speed(ShowProgress).zToStr()}MB/sec");

            // Disk
            _Debug.WriteLine("  " + SystemMeasurement.IO(enCounter_IO.Total_Transer));
            var diskAvgSecRead = new PerformanceCounter("PhysicalDisk", "Disk Transfers/sec", "_Total");
            _Debug.WriteLine($"Disk Transfer:{diskAvgSecRead.NextValue()}");
            System.Threading.Thread.Sleep(100);
            _Debug.WriteLine($"Disk Transfer:{diskAvgSecRead.NextValue()}");
            _Debug.WriteLine("  " + SystemMeasurement.IO(enCounter_IO.Total_Transer));
        }

        [Fact]
        [Test_Method("Letters(enIO_DriveType.Writeable)()")]
        [Test_Method("Drive_Speed()")]
        public void IO_DriveSpeed_Test()
        {
            var letters = _lamed.lib.IO.Drive.Letters(enIO_DriveType.Writeable);
            foreach (string driveLetter in letters)
            {
                _Debug.WriteLine($"Drive {driveLetter}: {_lamed.lib.IO.Drive.Drive_Speed(ShowProgress, driveLetter).zToStr()}MB/sec");
            }
            // This method should be successful

            //var mbPerSec2 = _lamed.lib.IO.Drive.Drive_Speed(ShowProgress, "e:\\");
            //_Debug.WriteLine($"Drive e: {_lamed.Types.Convert.Str_FromDouble(mbPerSec2)}MB/sec");
        }
        private void ShowProgress(object sender, int progress, string message)
        {
            _Debug.WriteLine("  "+message + $" ({progress}%)");
        }

        [Fact]
        public void IO_Disk_Test()
        { 
            // Disk
            var diskAvgSecRead = new PerformanceCounter("PhysicalDisk", "Disk Transfers/sec", "_Total");
            diskAvgSecRead.NextValue();
            var diskAvgSecWrite = new PerformanceCounter("PhysicalDisk", "Disk Reads/sec", "_Total");
            diskAvgSecWrite.NextValue();
            var diskCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
            diskCounter.NextValue();
            _Debug.WriteLine($"Disk read:{diskAvgSecRead.NextValue()}");
            _Debug.WriteLine($"Disk write:{diskAvgSecWrite.NextValue()}");
            _Debug.WriteLine($"Disk load:{diskCounter.NextValue()}%");

            // CPU
            var cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpu.NextValue();
            for (int iiIndex = 0; iiIndex < 2; iiIndex++)
            {
                System.Threading.Thread.Sleep(500);
                _Debug.WriteLine($"Total CPU:{cpu.NextValue()}%");
                _Debug.WriteLine($"Disk load:{diskCounter.NextValue() * 10}%".NL());
            }

            for (int iiIndex = 0; iiIndex < 3; iiIndex++)
            {
                System.Threading.Thread.Sleep(500);
                _Debug.WriteLine($"Disk read:{diskAvgSecRead.NextValue()}");
                _Debug.WriteLine($"Disk write:{diskAvgSecWrite.NextValue()}");
                _Debug.WriteLine($"Disk load:{diskCounter.NextValue()}%");
            }
        }

        [Fact]
        [Test_Method("Space_Total()")]
        [Test_Method("Space_Free()")]
        public void IO_Disk2_Test()
        {
            _Debug.WriteLine($"PCName:{Environment.MachineName}");
            var driveName = "C:\\";
            Assert.True(_lamed.lib.IO.Drive.Space_Total(driveName) > _lamed.lib.IO.Drive.Space_Free(driveName));
            long persent = _lamed.lib.IO.Drive.Space_FreePercent(driveName);
            Assert.True(persent > 10, $"Error! There is only {persent}% space left on drive.");
            _Debug.WriteLine($"Total disk space: {_lamed.lib.IO.Drive.Space_Total(driveName)}GB.");
            _Debug.WriteLine($"Free disk space: {_lamed.lib.IO.Drive.Space_Free(driveName)}GB.");
            _Debug.WriteLine($"Free space: {persent}%");

            var letters = _lamed.lib.IO.Drive.Letters();
            if (Environment.MachineName == "LAMEDALPC")
            {
                if (letters.Length == 2)
                {
                    Assert.True(_lamed.lib.IO.Drive.IsWritable("c:\\"));
                    Assert.False(_lamed.lib.IO.Drive.IsWritable("d:\\")); // CD drive
                }
                else
                {
                    Assert.Equal(3, letters.Length);
                    Assert.True(_lamed.lib.IO.Drive.IsWritable("c:\\"));
                    Assert.True(_lamed.lib.IO.Drive.IsWritable("e:\\"));
                    Assert.False(_lamed.lib.IO.Drive.IsWritable("d:\\"));  // CD drive
                }
            }
            else
            {
                _Debug.WriteLine($"PCName:{Environment.MachineName}");
                Assert.Equal(7, letters.Length);
            }
        }
       
        [Fact]
        [Test_Method("Performance_CategoryNames()")]
        [Test_Method("Performance_Counters()")]
        public void Info_Counters_Test()
        {
            // Get all catagories
            var categories = SystemMeasurement.Info_CategoryNames();
            _Debug.WriteLine($"No of categories: {categories.Count}");
        }
        
        [Fact]
        public void Info_Couters_Identify_Test()
        {
            List<string> counterlines;

            ////SystemMeasurement.Info_Couters("Memory", false);
            //List<PerformanceCounter> counters = SystemMeasurement.Info_Couters("Paging File");
            //Assert.Equal(4, counters.Count);

            //_Debug.WriteLine("Paging File:");
            //_Debug.WriteLine("==============");
            //SystemMeasurement.Info_Couters(out counterlines, "Paging File", true);
            //_Debug.WriteLine(counterlines.zTo_Str("".NL()));

            //_Debug.WriteLine("Memory:");
            //_Debug.WriteLine("=========");
            //SystemMeasurement.Info_Couters(out counterlines, "Memory");
            //_Debug.WriteLine(counterlines.zTo_Str("".NL()));

            //_Debug.WriteLine("PhysicalDisk:");
            //_Debug.WriteLine("==============");
            //SystemMeasurement.Info_Couters(out counterlines, "PhysicalDisk", false, false);
            //_Debug.WriteLine(counterlines.zTo_Str("".NL()));

            // "Processor"
            _Debug.WriteLine("Processor:");
            _Debug.WriteLine("==============");
            SystemMeasurement.Info_Couters(out counterlines, "Processor", false, false);
            _Debug.WriteLine(counterlines.zTo_Str("".NL()));
        }

        [Fact]
        public void Eventlog_Test()
        {

            var logAll = _lamedWin.lib.System.EventLog(filterLog:false);
            var logSystem = _lamedWin.lib.System.EventLog(logName:enEventLog.system);
            var logSystemToday = _lamedWin.lib.System.EventLog(logName: enEventLog.system, today: true);
            var logApplication = _lamedWin.lib.System.EventLog(logName:enEventLog.application);
            var logApplicationToday = _lamedWin.lib.System.EventLog(logName:enEventLog.application, today:true);

            _Debug.WriteLine($"All Errors: {logAll.Count}");
            _Debug.WriteLine($"System Errors: {logSystem.Count}");
            _Debug.WriteLine($"System Errors Today: {logSystemToday.Count}");
            _Debug.WriteLine($"Application Errors: {logApplication.Count}");
            _Debug.WriteLine($"Application Errors Today: {logApplicationToday.Count}");
            _Debug.WriteLine("---------------------------------[System errors");
            foreach (EventLogEntry logEntry in logSystemToday)
            {
                _Debug.WriteLine($"  Error Source: {logEntry.Source}");

            }
            _Debug.WriteLine("---------------------------------[Application errors");
            foreach (EventLogEntry logEntry in logApplicationToday)
            {
                _Debug.WriteLine($"  Error Source: {logEntry.Source}");

            }
        }

        [Fact]
        public void CPU_Test()
        {
            //cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            //cpuPrivilegedTime = new PerformanceCounter("Processor", "% Privileged Time", "_Total");
            //cpuInterruptTime = new PerformanceCounter("Processor", "% Interrupt Time", "_Total");
            //cpuDPCTime = new PerformanceCounter("Processor", "% DPC Time", "_Total");
            /*
Processor:
==============
  ("Processor", "% Processor Time","_Total,") = 0
  ("Processor", "% User Time","_Total,") = 0
  ("Processor", "% Privileged Time","_Total,") = 0

  ("Processor", "Interrupts/sec","_Total,") = 47662.81
  ("Processor", "% DPC Time","_Total,") = 0
  ("Processor", "% Interrupt Time","_Total,") = 0
  ("Processor", "DPCs Queued/sec","_Total,") = 21242.97
  ("Processor", "DPC Rate","_Total,") = 135
  ("Processor", "% Idle Time","_Total,") = 0
  ("Processor", "% C1 Time","_Total,") = 0
  ("Processor", "% C2 Time","_Total,") = 3.89817
  ("Processor", "% C3 Time","_Total,") = 0
  ("Processor", "C1 Transitions/sec","_Total,") = 20929.42
  ("Processor", "C2 Transitions/sec","_Total,") = 0
  ("Processor", "C3 Transitions/sec","_Total,") = 0              
            */
        }
    }
}