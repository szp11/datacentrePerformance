# ![alt tag](https://github.com/perezLamed/LamedalCore/blob/master/pics/badges/LamedalSmall.png) datacentrePerformance (version 0.0.1-alfa)
Track System Health and Performance (Errors / CPU / RAM / Memory / Disk / SQL Server) accross the organisation. This is an Open Source project under MIT License.

## Background
During 2017 a big global organisation experienced an email outage in one of their region's that lasted more than a week.  About 3 years before the incident the IT manager’s post was made redundant.  The IT support team was left behind without a leader and escalated all their problems (mostly small ones) resulting in Management not identifying or addressing the real risks when it started to show. No one was alarmed when the backups started to fail. No one listened to the reports that the server was running out of disk space. 

I have found that with any computer or server crash, there was clear warning signs that was ignored. I started searching for possible open source solutions for the Windows platform that will show both health and performance that can be used on 10-1000+ servers. The solution also need to be free and able to show both health and performance in an easy manner. I would like the solution to answer the following questions:
+ How healthy are the computers and servers in my organisation?
+ How is the performance of computers and servers?
+ Where are the bottle necks?
+ What indications exist of failure?

As I did not find anything that did meet the above requirements, I stared this project to fill this gap. 

## Objectives:
+ Only focus on the most important and valuable measurements. (There exist more than 20 000 measures on an average windows system.  This library only focus on measures with a clear use case for health or performance).
+ Implement health and performance measures on .Net Core platform if possible. If the measure cannot be implement on .Net Core, then implement on windows platform.
+ Port the full solution to .Net Core at the earliest convenience. (Currently many performance measures are still excluded from .Net Core).
+ Create a mechanism to take measures at predefined or custom defined intervals and write results to log files.
+ Simple display of measures on windows and console application. (Display of the measures for more than one system is currenlty out of scope for this project).

## Performance measures implemented:
Here is a list of the measures that are available and the reasons why they are important. If a measure can be implemented on .Net Core it will be done in the [LamedalCore library](https://github.com/perezLamed/LamedalCore) and reused here. 

**CPU:**
+  CPU performance.
+ CPU average performance: 
  - When the average CPU performance is above 80% it is an indication things are not in order.

**Memory:**
+ Total memory, available memory and memory usage
+ Page file usage, Max page file usage

**Disk:**
+ Disk speed
+ Disk total space and open space
+ Based on history usage, prediction when disk will run out of space (in progress).
+ Average disk read/write

**Errors:**
+ Capture system and application log messages and add filtering to identify possible problems:
  - Show total unique errors per day or for a  time period.
  
## Usage:
(This section will be populated once the first version is released on NuGet)

MIT license
