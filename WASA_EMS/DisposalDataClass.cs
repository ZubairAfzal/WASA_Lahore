using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WASA_EMS
{
    public class DisposalDataClass
    {
        public string locationName1 { get; set; }
        public string locationName2 { get; set; }
        public List<double> PumpStatus1 { get; set; }
        public List<string> Pump1TimeArray { get; set; }
        public List<double> PumpStatus2 { get; set; }
        public List<double> PumpStatus3 { get; set; }
        public List<double> PumpStatus4 { get; set; }
        public List<double> PumpStatus5 { get; set; }
        public List<double> PumpStatus6 { get; set; }
        public List<string> Pump6TimeArray { get; set; }
        public List<double> PumpStatus7 { get; set; }
        public List<double> PumpStatus8 { get; set; }
        public List<double> PumpStatus9 { get; set; }
        public List<double> PumpStatus10 { get; set; }
        public string WorkingInHoursPump1 { get; set; }
        public string WorkingInHoursPump2 { get; set; }
        public string WorkingInHoursPump3 { get; set; }
        public string WorkingInHoursPump4 { get; set; }
        public string WorkingInHoursPump5 { get; set; }
        public string WorkingInHoursPump6 { get; set; }
        public string WorkingInHoursPump7 { get; set; }
        public string WorkingInHoursPump8 { get; set; }
        public string WorkingInHoursPump9 { get; set; }
        public string WorkingInHoursPump10 { get; set; }
        public double WorkingHoursPump1 { get; set; }
        public double WorkingHoursPump2 { get; set; }
        public double WorkingHoursPump3 { get; set; }
        public double WorkingHoursPump4 { get; set; }
        public double WorkingHoursPump5 { get; set; }
        public double WorkingHoursPump6 { get; set; }
        public double WorkingHoursPump7 { get; set; }
        public double WorkingHoursPump8 { get; set; }
        public double WorkingHoursPump9 { get; set; }
        public double WorkingHoursPump10 { get; set; }
        public List<double> Well1Level { get; set; }
        public string Well1Level_Average { get; set; }
        public List<double> Well2Level { get; set; }
        public string Well2Level_Average { get; set; }
    }
}