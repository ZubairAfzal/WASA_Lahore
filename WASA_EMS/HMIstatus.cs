using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WASA_EMS
{
    public class HMIstatus
    {
        public double pumpStatus { get; set; }
        public double chlorineStatus { get; set; }
        public double primingStatus { get; set; }
        public double flowRate { get; set; }
        public double pressureRate { get; set; }
        public double v1n { get; set; }
        public double v2n { get; set; }
        public double v3n { get; set; }
        public string lastTime { get; set; }
        public double delTime { get; set; }
    }

    public class DisposalHMIstatus
    {
        public double pumpStatus1 { get; set; }
        public double pumpStatus2 { get; set; }
        public double pumpStatus3 { get; set; }
        public double pumpStatus4 { get; set; }
        public double pumpStatus5 { get; set; }
        public double pumpStatus6 { get; set; }
        public double pumpStatus7 { get; set; }
        public double pumpStatus8 { get; set; }
        public double pumpStatus9 { get; set; }
        public double pumpStatus10 { get; set; }
        public double wellLevel1 { get; set; }
        public double wellLevel2 { get; set; }
    }
}