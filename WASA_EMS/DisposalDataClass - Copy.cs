using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WASA_EMS
{
    public class RecylcingPlantClass
    {
        public string LocationName { get; set; }

        public List<double> PumpStatus1 { get; set; }
        public List<double> PumpStatus2 { get; set; }
        public List<double> PumpStatus3 { get; set; }

        public List<string> PumpTimeArray { get; set; }

        public string WorkingInHoursPump1 { get; set; }
        public string WorkingInHoursPump2 { get; set; }
        public string WorkingInHoursPump3 { get; set; }

        public double WorkingHoursPump1 { get; set; }
        public double WorkingHoursPump2 { get; set; }
        public double WorkingHoursPump3 { get; set; }


    }

    public class RecyclePump1SpellData
    {
        public int SpellNumber { get; set; }
        public string SpellStartTime { get; set; }
        public string SpellEndTime { get; set; }
        public List<double> SpellDataArray = new List<double>();
        public List<string> SpellTimeArray = new List<string>();
        public int ResourceId { get; set; }
        public string resourceName { get; set; }
        public double spellPeriod { get; set; }
    }
    public class RecyclePump2SpellData
    {
        public int SpellNumber { get; set; }
        public string SpellStartTime { get; set; }
        public string SpellEndTime { get; set; }
        public List<double> SpellDataArray = new List<double>();
        public List<string> SpellTimeArray = new List<string>();
        public int ResourceId { get; set; }
        public string resourceName { get; set; }
        public double spellPeriod { get; set; }
    }
    public class RecyclePump3SpellData
    {
        public int SpellNumber { get; set; }
        public string SpellStartTime { get; set; }
        public string SpellEndTime { get; set; }
        public List<double> SpellDataArray = new List<double>();
        public List<string> SpellTimeArray = new List<string>();
        public int ResourceId { get; set; }
        public string resourceName { get; set; }
        public double spellPeriod { get; set; }
    }
}