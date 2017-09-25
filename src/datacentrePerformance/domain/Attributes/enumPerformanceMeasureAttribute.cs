using System;
using System.Reflection;
using LamedalCore.domain.Attributes;
using LamedalCore.domain.Enumerals;

namespace datacentrePerformance.domain.Attributes
{
    /// <summary>
    /// This attribute is used to define a performance counter.
    /// </summary>
    [BlueprintRule_Class(enBlueprint_ClassNetworkType.Node_Data)]
    public class enumPerformanceMeasureAttribute : Attribute
    {
        public string Name_Category;
        public string Name_Counter;
        public string Name_Instance;
        public string Help;
        public string Name_Caption;   // The caption that can be used
        public Enum Id;

        public float Value_;
        public string Value_AsStr;
        public string Value_Unit;  // %, MB

        // Parameters to convert result to more usable output format
        public int Format_Multiplier = 1;       // The number to multiply of devide the number with
        public int Format_DecimalPlaces = 0;    // Number of decimal places
        public int Format_NextValue2Use = 1;    // What value to use
        public int Format_NextValueSleep = 100;  // Sleep in milliseconds between values
        public bool Format_IsMultiplier = true;  // If true then multiply else devide

        // Alarm values
        // Safe region
        public float Alarm_SafeMin = -1;
        public float Alarm_SafeMax = -1;

        // Warning region
        public float Alarm_WarningMin = -1;
        public float Alarm_WarningMax = -1;

        // Error region
        public float Alarm_ErrorMin = -1;
        public float Alarm_ErrorMax = -1;

        /// <summary>
        /// Defines a PerformanceCounter for a enumerable value.
        /// </summary>
        /// <param name="categoryName">The category name of the performance counter</param>
        /// <param name="counterName">The counter name</param>
        /// <param name="instanceName">The instance name</param>
        /// <param name="caption">The caption that can be shown</param>
        /// <param name="unit">The unit of the measurement</param>
        /// <param name="isMultiplier">If [true] then use multiplier. If [false] use divider</param>
        /// <param name="multiplier">The multiplier. Useful to scale the result</param>
        /// <param name="decimalPlaces">The number of decimal places</param>
        /// <param name="nextValue2Use">If [false] then the 2nd value of the performance counter will be used</param>
        /// <param name="help">The help string for the measure</param>
        public enumPerformanceMeasureAttribute(string categoryName, string counterName, string instanceName = "", int nextValue2Use = 1,
                    string caption = "", string unit = "", 
                    bool isMultiplier = true, int multiplier = 1, int decimalPlaces = 0,
                    string help = "")
        {
            Name_Category = categoryName;
            Name_Counter = counterName;
            Name_Instance = instanceName;
            Name_Caption = caption;
            Format_Multiplier = multiplier;
            Format_IsMultiplier = isMultiplier;
            Format_DecimalPlaces = decimalPlaces;
            Format_NextValue2Use = nextValue2Use;
            Value_Unit = unit;
            Help = help;
        }

        /// <summary>
        /// Defines a PerformanceCounter for a enumerable value. This constructor is used to set custom fields in a more discriptive manner.
        /// </summary>
        public enumPerformanceMeasureAttribute() {}

        public override string ToString()
        {
            return $"{Name_Caption}: {Value_AsStr}{Value_Unit}";
        }
    }

    /// <summary>
    /// Helper class to retrieve the attribute for the enumerable
    /// </summary>
    public static class enumPerformanceMeasureAttribute_Helper
    {
        /// <summary>
        /// Function to get the value of the enumerator.
        /// </summary>
        /// <param name="enumerable">The enumber</param>
        /// <returns></returns>
        public static enumPerformanceMeasureAttribute zAttribute_AsPerformanceMeasure(this Enum enumerable)
        {
            var enumValueAttributes = (enumPerformanceMeasureAttribute[])(enumerable.GetType().GetTypeInfo().GetField(enumerable.ToString())).GetCustomAttributes(typeof(enumPerformanceMeasureAttribute), false);
            if (enumValueAttributes.Length > 0)
            {
                enumPerformanceMeasureAttribute result = enumValueAttributes[0];
                result.Id = enumerable;
                return result;
            }

            throw new InvalidOperationException($"Error! Unable to find enumPerformanceMeasureAttribute for '{enumerable}'.");
        }
    }
}