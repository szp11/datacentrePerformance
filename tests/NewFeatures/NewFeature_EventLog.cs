using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LamedalCore;
using LamedalCore.domain.Enumerals;

namespace datacentrePerformance.Test.NewFeatures
{
    public class NewFeature_EventLog
    {
        private static readonly LamedalCore_ _lamed = LamedalCore_.Instance; // system library

        /// <summary>
        /// Return the eventlog and filter it.
        /// </summary>
        /// <param name="filterLog">If [true] then filter the log</param>
        /// <param name="logName">The event log name</param>
        /// <param name="unique">if [true] then the list is unique</param>
        /// <param name="today">if [true] filter for today</param>
        /// <param name="eventFilterType">The event filter type</param>
        /// <returns></returns>
        public static List<EventLogEntry> EventLog(bool filterLog = true, enEventLog logName = enEventLog.system, bool unique = true, bool today = false, EventLogEntryType eventFilterType = EventLogEntryType.Error)
        {
            //var log = "application";
            var log = logName.ToString();
            var aLog = new EventLog(log, Environment.MachineName);
            //EventLogEntry entry;
            List<EventLogEntry> result = aLog.Entries.Cast<EventLogEntry>().ToList();
            if (filterLog) result = result.Where(x => x.EntryType == eventFilterType).ToList();
            if (today) result = result.Where(x => _lamed.Types.DateTime.IsToday(x.TimeGenerated)).ToList();
            if (unique)
            {
                var uniqueList = new Dictionary<string, EventLogEntry>();
                foreach (var entry in result)
                {
                    _lamed.Types.Dictionary.Key_AddSafe(uniqueList, entry.Source, entry, enCompare_DuplicateAction.Ignore);
                    result = uniqueList.Values.ToList();
                }
            }
            return result;
        }

    }
}