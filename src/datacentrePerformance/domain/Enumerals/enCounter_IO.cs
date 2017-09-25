using datacentrePerformance.domain.Attributes;

namespace datacentrePerformance.domain.Enumerals
{
    public enum enCounter_IO
    {
        [enumPerformanceMeasure(
            Name_Category = "PhysicalDisk",
            Name_Counter = "Disk Reads/sec",
            Name_Instance = "_Total",
            Name_Caption = "Total reads per second", 
            Value_Unit = "MB/sec",
            Format_NextValue2Use = 2)]
        Total_Read,

        [enumPerformanceMeasure(
        Name_Category = "PhysicalDisk",
        Name_Counter = "Disk Writes/sec",
        Name_Instance = "_Total",
        Name_Caption = "Total writes per second",
        Value_Unit = "MB/sec",
        Format_NextValue2Use = 2)]
        Total_Write,

        [enumPerformanceMeasure(
            Name_Category = "PhysicalDisk",
            Name_Counter = "Disk Transfers/sec",
            Name_Instance = "_Total",
            Name_Caption = "Total transfer per second",
            Value_Unit = "MB/sec",
            Format_NextValue2Use = 2,
            Format_NextValueSleep = 500)]
        Total_Transer,

        [enumPerformanceMeasure(
            Name_Category = "PhysicalDisk",
            Name_Counter = "% Disk Time",
            Name_Instance = "_Total",
            Name_Caption = "Total disk time",
            Value_Unit = "%",
            Format_NextValue2Use = 2)]
        Total_DiskTimePercent,

        [enumPerformanceMeasure(
            Name_Category = "IO",
            //Name_Instance = "c:\\",
            Name_Caption = "Total disk space for '{0}'",
            Value_Unit = "GB",
            Format_Multiplier = 1024 * 1024 * 1024,
            Format_IsMultiplier = false,
            Help = "This measure show the total size of a specific disk.")]
        Space_Total,

        [enumPerformanceMeasure(
            Name_Category = "IO",
            //Name_Instance = "c:\\",
            Name_Caption = "Free disk space for '{0}'",
            Value_Unit = "GB",
            Format_Multiplier = 1024*1024*1024,
            Format_IsMultiplier = false,
            Help = "This measure show the amount of space that is available for a specific disk.")]
        Space_Free,

        [enumPerformanceMeasure(
            Name_Category = "IO",
            //Name_Instance = "c:\\",
            Name_Caption = "Disk space used for '{0}'",
            Value_Unit = "GB",
            Format_Multiplier = 1024 * 1024 * 1024,
            Format_IsMultiplier = false,
            Help = "This measure show the amount of space that is available for a specific disk.")]
        Space_Used,

        [enumPerformanceMeasure(
            Name_Category = "IO",
            //Name_Instance = "c:\\",
            Name_Caption = "% free disk space for '{0}'",
            Value_Unit = "%",
            Help = "This measure show the % of space that is available for a specific disk.")]
        Space_FreePercent,

        [enumPerformanceMeasure(
            Name_Category = "IO",
            //Name_Instance = "c:\\",
            Name_Caption = "Disk speed for '{0}'",
            Value_Unit = "MB/sec",
            Help = "This measure show the speed of a specific disk.")]
        Speed
    }
}