using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WASA_EMS
{
    public class TubewellDataClass
    {
        [Display(Name = "Location Name")]
        public string locationName { get; set; }
        public string Specification { get; set; }
        public string WaterLevel_m { get; set; }
        public string PumpingWaterLevel_hpl { get; set; }
        public string RatedDischarge_Q { get; set; }
        public string RatedHead_H { get; set; }
        public string Discharge_Dia_Dd { get; set; }
        public double WorkingInHours { get; set; }
        public double WorkingInHoursRemote { get; set; }
        public double WorkingInHoursManual { get; set; }
        public double WorkingInHoursScheduling { get; set; }
        public List<double> pumpStatus { get; set; }
        public string workingHoursToday { get; set; }
        public string workingHoursTodayRemote { get; set; }
        public string workingHoursTodayManual { get; set; }
        public string workingHoursTodayScheduling { get; set; }
        public List<double> waterFlow { get; set; }
        public string waterDischarge { get; set; }
        public string accWaterDischargePerDay { get; set; }
        public List<double> chlorineLevel { get; set; }
        public List<double> powerFactor { get; set; }
        public List<double> V1N { get; set; }
        public List<double> V2N { get; set; }
        public List<double> V3N { get; set; }
        public List<double> V12 { get; set; }
        public List<double> V13 { get; set; }
        public List<double> V23 { get; set; }
        public string averageVoltage { get; set; }
        public List<double> voltageTrip { get; set; }
        public List<double> I1 { get; set; }
        public List<double> I2 { get; set; }
        public List<double> I3 { get; set; }
        public string averageCurrent { get; set; }
        public List<double> currentTrip { get; set; }
        public List<double> frequency { get; set; }
        public List<double> pkva { get; set; }
        public List<double> pkvar { get; set; }
        public List<double> pkw { get; set; }
        public List<double> autoMode { get; set; }
        public List<double> remoteControll { get; set; }
        public List<double> schedulingStatus { get; set; }
        public List<double> manualStatus { get; set; }
        public List<double> primingTankLevel { get; set; }
        public List<double> pressure { get; set; }
        public string chartString { get; set; }
        public string dateOfData { get; set; }
        public List<double> Vibration_m { get; set; }
        public List<double> Vibration_m_s { get; set; }
        public List<double> Vibration_m_s_2 { get; set; }
        public List<string> LogTime { get; set; }
        public string logDate { get; set; }
        public int noOfDays { get; set; }
    }   
}