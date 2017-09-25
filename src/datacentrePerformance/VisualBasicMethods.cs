using JetBrains.Annotations;
using LamedalCore.domain.Attributes;
using Microsoft.VisualBasic.Devices;

namespace datacentrePerformance
{
    internal static class VisualBasicMethods
    {
        /// <summary>
        /// Total Physical Memory
        /// </summary>
        internal static long Memory_PhysicalTotal()
        {
            if (_computer == null) _computer = new ComputerInfo();
            return (long)_computer.TotalPhysicalMemory;
        }

        /// <summary>
        /// Physical Memory Available
        /// </summary>
        internal static long Memory_PhysicalAvailable()
        {
            if (_computer == null) _computer = new ComputerInfo();
            return (long)_computer.AvailablePhysicalMemory;
        }

        /// <summary>
        /// Physical Memory in use
        /// </summary>
        internal static long Memory_PhysicalInUse()
        {
            var total = Memory_PhysicalTotal();
            var available = Memory_PhysicalAvailable();
            return total - available;
        }

        /// <summary>
        /// Total Virtual Memory
        /// </summary>
        internal static long Memory_VirtualTotal()
        {
            if (_computer == null) _computer = new ComputerInfo();
            return (long)_computer.TotalVirtualMemory;
        }

        /// <summary>
        /// Available Virtual Memory
        /// </summary>
        internal static long Memory_VirtualAvailable()
        {
            if (_computer == null) _computer = new ComputerInfo();
            return (long)_computer.AvailableVirtualMemory;
        }

        /// <summary>
        /// Virtual Memory in use
        /// </summary>
        internal static long Memory_VirtualInUse()
        {
            var total = Memory_VirtualTotal();
            var available = Memory_VirtualAvailable();
            return total - available;
        }
        private static ComputerInfo _computer;

    }
}