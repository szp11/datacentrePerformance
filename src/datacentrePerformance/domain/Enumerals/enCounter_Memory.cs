using datacentrePerformance.domain.Attributes;

namespace datacentrePerformance.domain.Enumerals
{
    public enum enCounter_Memory
    {
        [enumPerformanceMeasure(
            Name_Category = "Memory",
            Name_Caption =  "Active process memory", 
            Value_Unit = "MB", 
            Format_IsMultiplier = false, 
            Format_Multiplier = 1024 * 1024, 
            Help = "This measure show the memory used by the active process. It can be used to track internal operations that use lots of memory.")
        ]
        activeProcessMemory,

        [enumPerformanceMeasure(
            Name_Category = "Memory",
            Name_Caption = "Available Physical memory", Value_Unit = "GB", 
            Format_IsMultiplier = false, Format_Multiplier = 1024 * 1024 * 1024, Format_DecimalPlaces = 1)
        ]
        PhysicalMemory_Available,

        [enumPerformanceMeasure(
            Name_Category = "Memory",
            Name_Caption = "Total Physical memory", Value_Unit = "GB",
            Format_IsMultiplier = false, Format_Multiplier = 1024 * 1024 * 1024, Format_DecimalPlaces = 1)
        ]
        PhysicalMemory_Total,

        [enumPerformanceMeasure(
            Name_Category = "Memory",
            Name_Caption = "Physical memory in use", Value_Unit = "GB",
            Format_IsMultiplier = false, Format_Multiplier = 1024 * 1024 * 1024, Format_DecimalPlaces = 1)
        ]
        PhysicalMemory_InUse,

        [enumPerformanceMeasure(
            Name_Category = "Memory",
            Name_Caption = "Available Virtual memory", Value_Unit = "GB",
            Format_IsMultiplier = false, Format_Multiplier = 1024 * 1024 * 1024, Format_DecimalPlaces = 1)
        ]
        VirtualMemory_Available,

        [enumPerformanceMeasure(
            Name_Category = "Memory",
            Name_Caption = "Total Virtual memory", Value_Unit = "GB",
            Format_IsMultiplier = false, Format_Multiplier = 1024 * 1024 * 1024, Format_DecimalPlaces = 1)
        ]
        VirtualMemory_Total,

        [enumPerformanceMeasure(
            Name_Category = "Memory",
            Name_Caption = "Virtual memory in use", Value_Unit = "GB",
            Format_IsMultiplier = false, Format_Multiplier = 1024 * 1024 * 1024, Format_DecimalPlaces = 1)
        ]
        VirtualMemory_InUse,

        [enumPerformanceMeasure("Paging File", "% Usage", "_Total", 1, "Paging file usage", "%", true, 100)]
        pagingFileUsage,

        [enumPerformanceMeasure("Paging File", "% Usage Peak", "_Total", 1, "Paging file peak", "%", true, 100)]
        pagingFilePeak,
    }
}