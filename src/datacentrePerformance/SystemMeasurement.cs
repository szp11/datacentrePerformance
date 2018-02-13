using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using datacentrePerformance.domain.Attributes;
using datacentrePerformance.domain.Enumerals;
using JetBrains.Annotations;
using LamedalCore;
using LamedalCore.domain.Attributes;
using LamedalCore.domain.Enumerals;
using LamedalCore.zz;

namespace datacentrePerformance
{
    [BlueprintRule_Class(enBlueprint_ClassNetworkType.Node_Action)]
    public static class SystemMeasurement
    {
        private static readonly LamedalCore_ _lamed = LamedalCore_.Instance; // system library
        private static readonly Dictionary<Enum, PerformanceCounter> _performanceCounters = new Dictionary<Enum, PerformanceCounter>(); // Save counters for fast reference

        /// <summary>
        /// Return memory usage for a specific counter
        /// </summary>
        /// <param name="performance"></param>
        /// <returns></returns>
        public static void Memory(enumPerformanceMeasureAttribute performance)
        {
            if (performance.Name_Category != "Memory" && 
                performance.Name_Category != "Paging File") throw new ArgumentException($"Error! '{performance.Name_Category}' is not a valid 'Memory' counter.", nameof(performance));

            switch (performance.Id)
            {
                case enCounter_Memory.activeProcessMemory: performance.Value_ = _lamed.lib.System.Runtime.activeProcessMemory(); break;
                case enCounter_Memory.PhysicalMemory_Available: performance.Value_ = VisualBasicMethods.Memory_PhysicalAvailable(); break;
                case enCounter_Memory.PhysicalMemory_Total: performance.Value_ = VisualBasicMethods.Memory_PhysicalTotal(); break;
                case enCounter_Memory.PhysicalMemory_InUse: performance.Value_ = VisualBasicMethods.Memory_PhysicalInUse(); break;
                case enCounter_Memory.VirtualMemory_Available: performance.Value_ = VisualBasicMethods.Memory_VirtualAvailable(); break;
                case enCounter_Memory.VirtualMemory_Total: performance.Value_ = VisualBasicMethods.Memory_VirtualTotal(); break;
                case enCounter_Memory.VirtualMemory_InUse: performance.Value_ = VisualBasicMethods.Memory_VirtualInUse(); break;
                default: WindowsPerformanceCounter(performance); break;
            }
            // Format the value
            FormatValue(performance);
        }

        /// <summary>
        /// Return memory usage for a specific counter
        /// </summary>
        /// <param name="counterType"></param>
        /// <returns></returns>
        [Pure]
        public static enumPerformanceMeasureAttribute Memory(enCounter_Memory counterType)
        {
            //PerformanceCounter result;
            enumPerformanceMeasureAttribute result = counterType.zAttribute_AsPerformanceMeasure();
            Memory(result);
            return result;
        }


        #region private
        private static void FormatValue(enumPerformanceMeasureAttribute performance)
        {
            var multiplier = (float) performance.Format_Multiplier;
            if (performance.Format_IsMultiplier)
                performance.Value_ = performance.Value_ * multiplier;
            else performance.Value_ = performance.Value_ / multiplier;

            performance.Value_AsStr = _lamed.Types.Convert.Str_FromDouble(performance.Value_, performance.Format_DecimalPlaces);
            // Merge the instance with the caption
            if (performance.Name_Caption.Contains("{0}"))
                performance.Name_Caption = performance.Name_Caption.zFormat(performance.Name_Instance);
        }

        private static PerformanceCounter WindowsPerformanceCounter([NotNull] enumPerformanceMeasureAttribute performance)
        {
            PerformanceCounter performanceCounter;
            if (_performanceCounters.TryGetValue(performance.Id, out performanceCounter) == false)
            {
                if (performance.Name_Instance == "")
                    performanceCounter = new PerformanceCounter(performance.Name_Category, performance.Name_Counter);
                else
                    performanceCounter = new PerformanceCounter(performance.Name_Category, performance.Name_Counter,
                        performance.Name_Instance);
                _performanceCounters.Add(performance.Id, performanceCounter);
            }

            // Get the counter value
            float valueMax = 0;
            float value = 0;
            if (performance.Format_NextValue2Use != 1)
            {
                for (int ii = 1; ii < performance.Format_NextValue2Use; ii++)
                {
                    value = performanceCounter.NextValue();
                    if (value > valueMax) valueMax = value;
                    Thread.Sleep(performance.Format_NextValueSleep);
                }
            }
            value = performanceCounter.NextValue();
            if (value > valueMax) valueMax = value;
            performance.Value_ = valueMax;
            if (performance.Help == "") performance.Help = performanceCounter.CounterHelp;
            return performanceCounter;
        }
        #endregion

        /// <summary>
        /// Return the IO performance measure
        /// </summary>
        /// <param name="performance"></param>
        public static void IO(enumPerformanceMeasureAttribute performance)
        {
            if (performance.Name_Category != "IO" && 
                performance.Name_Category != "PhysicalDisk") throw new ArgumentException($"Error! '{performance.Name_Category}' is not a valid IO counter.", nameof(performance));

            switch (performance.Id)
            {
                case enCounter_IO.Space_Free: performance.Value_ = _lamed.lib.IO.Drive.Space_Free(performance.Name_Instance); break;
                case enCounter_IO.Space_FreePercent: performance.Value_ = _lamed.lib.IO.Drive.Space_FreePercent(performance.Name_Instance); break;
                case enCounter_IO.Space_Total: performance.Value_ = _lamed.lib.IO.Drive.Space_Total(performance.Name_Instance); break;
                case enCounter_IO.Space_Used: performance.Value_ = _lamed.lib.IO.Drive.Space_InUse(performance.Name_Instance); break;
                case enCounter_IO.Speed: performance.Value_ = (float)_lamed.lib.IO.Drive.Drive_Speed(null,performance.Name_Instance); break;
                default: WindowsPerformanceCounter(performance); break;
            }
            // Format the value
            FormatValue(performance);
        }

        /// <summary>
        /// Return IO usage for a specific counter
        /// </summary>
        /// <param name="counterType"></param>
        /// <param name="driveName">The drive name: eq. "c:\"</param>
        /// <returns></returns>
        [Pure]
        public static enumPerformanceMeasureAttribute IO(enCounter_IO counterType, string driveName = "")
        {
            //PerformanceCounter result;
            enumPerformanceMeasureAttribute result = counterType.zAttribute_AsPerformanceMeasure();
            if (driveName != "" && result.Name_Instance.zIsNullOrEmpty()) result.Name_Instance = driveName;
            IO(result);
            return result;
        }

        /// <summary>
        /// Return the performance counter category names.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.Contracts.Pure]
        public static List<string> Info_CategoryNames()
        {
            var categories = PerformanceCounterCategory.GetCategories();
            var result = categories.Select(x => x.CategoryName).ToList();
            return result;
        }

        /// <summary>
        /// For a category, return all performance couners
        /// </summary>
        /// <param name="catagory"></param>
        /// <returns></returns>
        public static List<PerformanceCounter> Info_Couters(string catagory = "")
        {
            List<string> counterLines;
            return Info_Couters(out counterLines, catagory);
        }

        /// <summary>
        /// For a category, return all performance couners
        /// </summary>
        /// <param name="counterLines"></param>
        /// <param name="catagory"></param>
        /// <param name="showHelp"></param>
        /// <param name="filter2NonZero"></param>
        /// <returns></returns>
        public static List<PerformanceCounter> Info_Couters(out List<string> counterLines, string catagory = "", bool showHelp = false, bool filter2NonZero = true)
        {
            counterLines = new List<string>();
            var result = new List<PerformanceCounter>();
            var counterCategories = PerformanceCounterCategory.GetCategories();
            foreach (var pc in counterCategories)
            {
                if (catagory == "" || pc.CategoryName == catagory)
                {
                    try
                    {
                        var counters = new List<PerformanceCounter>();
                        var instances = pc.GetInstanceNames();
                        if (instances.Length == 0) counters = pc.GetCounters().ToList();
                        else
                        {
                            foreach (var insta in instances)
                            {
                                List<PerformanceCounter> instanceCounters = pc.GetCounters(insta).ToList();
                                counters.AddRange(instanceCounters);
                            }
                        }
                        try
                        {
                            foreach (PerformanceCounter cntr in counters)
                            {
                                cntr.NextValue();
                                float value = cntr.NextValue();
                                if (filter2NonZero && _lamed.Types.Is.IsEqual2Float(value, 0)) continue; // Do not show counters that have no value --------------------------------
                                result.Add(cntr);
                                var instance = (cntr.InstanceName == "") ? "" : $",\"{cntr.InstanceName},\"";
                                var line1 = $"  (\"{pc.CategoryName}\", \"{cntr.CounterName}\"{instance}) = {value.zObject().AsStr()}";
                                var line2 = $"    {cntr.CounterHelp}";
                                var line3 = "---------------------------";
                                counterLines.Add(line1);
                                if (showHelp)
                                {
                                    counterLines.Add(line2);
                                    counterLines.Add(line3);
                                }
                            }
                        }
                        catch (Exception) { }
                    }
                    catch (Exception) { }
                }
            }
            return result;
        }
    }
}
