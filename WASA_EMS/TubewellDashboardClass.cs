using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WASA_EMS
{
    public class TubewellDashboardClass
    {
        public string locationName { get; set; }
        public string parameterName { get; set; }
        public string lastActiveTime { get; set; }
        public double totalHours { get; set; }
        public double workingInHours { get; set; }
        public double nonWorkingInHours { get; set; }
        public double workingInHoursRemote { get; set; }
        public double workingInHoursManual { get; set; }
        public double workingInHoursScheduling { get; set; }
        public double availableHours { get; set; }
        public double nonAvailableHours { get; set; }

        public string totalHoursString { get; set; }
        public string workingInHoursString { get; set; }
        public string nonWorkingInHoursString { get; set; }
        public string workingInHoursRemoteString { get; set; }
        public string workingInHoursManualString { get; set; }
        public string workingInHoursSchedulingString { get; set; }
        public string availableHoursString { get; set; }
        public string nonAvailableHoursString { get; set; }


        public double minValueV1N { get; set; }
        public double maxValueV1N { get; set; }
        public double avgValeV1N { get; set; }

        public double avgOfAvailableHoursV1N { get; set; }
        public double avgOfNonAvailableHoursV1N { get; set; }

        public double minValueV2N { get; set; }
        public double maxValueV2N { get; set; }
        public double avgValeV2N { get; set; }

        public double avgOfAvailableHoursV2N { get; set; }
        public double avgOfNonAvailableHoursV2N { get; set; }

        public double minValueV3N { get; set; }
        public double maxValueV3N { get; set; }
        public double avgValeV3N { get; set; }

        public double avgOfAvailableHoursV3N { get; set; }
        public double avgOfNonAvailableHoursV3N { get; set; }

        public double minValueI1 { get; set; }
        public double maxValueI1 { get; set; }
        public double avgValeI1 { get; set; }

        public double avgOfAvailableHoursI1 { get; set; }
        public double avgOfNonAvailableHoursI1 { get; set; }

        public double minValueI2 { get; set; }
        public double maxValueI2 { get; set; }
        public double avgValeI2 { get; set; }

        public double avgOfAvailableHoursI2 { get; set; }
        public double avgOfNonAvailableHoursI2 { get; set; }

        public double minValueI3 { get; set; }
        public double maxValueI3 { get; set; }
        public double avgValeI3 { get; set; }

        public double avgOfAvailableHoursI3 { get; set; }
        public double avgOfNonAvailableHoursI3 { get; set; }

        public double minValueFrequency { get; set; }
        public double maxValueFrequency { get; set; }
        public double avgValeFrequency { get; set; }

        public double avgOfAvailableHoursFrequency { get; set; }
        public double avgOfNonAvailableHoursFrequency { get; set; }

        public double minValuePKVA { get; set; }
        public double maxValuePKVA { get; set; }
        public double avgValePKVA { get; set; }

        public double avgOfAvailableHoursPKVA { get; set; }
        public double avgOfNonAvailableHoursPKVA { get; set; }

        public double minValuePF { get; set; }
        public double maxValuePF { get; set; }
        public double avgValePF { get; set; }

        public double avgOfAvailableHoursPF { get; set; }
        public double avgOfNonAvailableHoursPF { get; set; }

        public double minValueRemote { get; set; }
        public double maxValueRemote { get; set; }
        public double avgValeRemote { get; set; }

        public double avgOfAvailableHoursRemote { get; set; }
        public double avgOfNonAvailableHoursRemote { get; set; }

        public double minValuePumpStatus { get; set; }
        public double maxValuePumpStatus { get; set; }
        public double avgValePumpStatus { get; set; }

        public double avgOfAvailableHoursPumpStatus { get; set; }
        public double avgOfNonAvailableHoursPumpStatus { get; set; }

        public double minValueCurrentTrip { get; set; }
        public double maxValueCurrentTrip { get; set; }
        public double avgValeCurrentTrip { get; set; }

        public double avgOfAvailableHoursCurrentTrip { get; set; }
        public double avgOfNonAvailableHoursCurrentTrip { get; set; }

        public double minValueVoltageTrip { get; set; }
        public double maxValueVoltageTrip { get; set; }
        public double avgValeVoltageTrip { get; set; }

        public double avgOfAvailableHoursVoltageTrip { get; set; }
        public double avgOfNonAvailableHoursVoltageTrip { get; set; }

        public double minValueTimeSchedule { get; set; }
        public double maxValueTimeSchedule { get; set; }
        public double avgValeTimeSchedule { get; set; }

        public double avgOfAvailableHoursTimeSchedule { get; set; }
        public double avgOfNonAvailableHoursTimeSchedule { get; set; }

        public double minValueChlorineLevel { get; set; }
        public double maxValueChlorineLevel { get; set; }
        public double avgValeChlorineLevel { get; set; }

        public double avgOfAvailableHoursChlorineLevel { get; set; }
        public double avgOfNonAvailableHoursChlorineLevel { get; set; }

        public double minValueWaterFlow { get; set; }
        public double maxValueWaterFlow { get; set; }
        public double avgValeWaterFlow { get; set; }

        public double avgOfAvailableHoursWaterFlow { get; set; }
        public double avgOfNonAvailableHoursWaterFlow { get; set; }

        public double minValuePKVAR { get; set; }
        public double maxValuePKVAR { get; set; }
        public double avgValePKVAR { get; set; }

        public double avgOfAvailableHoursPKVAR { get; set; }
        public double avgOfNonAvailableHoursPKVAR { get; set; }

        public double minValuePKW { get; set; }
        public double maxValuePKW { get; set; }
        public double avgValePKW { get; set; }

        public double avgOfAvailableHoursPKW { get; set; }
        public double avgOfNonAvailableHoursPKW { get; set; }

        public double minValueV12 { get; set; }
        public double maxValueV12 { get; set; }
        public double avgValeV12 { get; set; }

        public double avgOfAvailableHoursV12 { get; set; }
        public double avgOfNonAvailableHoursV12 { get; set; }

        public double minValueV13 { get; set; }
        public double maxValueV13 { get; set; }
        public double avgValeV13 { get; set; }

        public double avgOfAvailableHoursV13 { get; set; }
        public double avgOfNonAvailableHoursV13 { get; set; }

        public double minValueV23 { get; set; }
        public double maxValueV23 { get; set; }
        public double avgValeV23 { get; set; }

        public double avgOfAvailableHoursV23 { get; set; }
        public double avgOfNonAvailableHoursV23 { get; set; }

        public double minValuePrimingLevel { get; set; }
        public double maxValuePrimingLevel { get; set; }
        public double avgValePrimingLevel { get; set; }

        public double avgOfAvailableHoursPrimingLevel { get; set; }
        public double avgOfNonAvailableHoursPrimingLevel { get; set; }

        public double minValuePressure { get; set; }
        public double maxValuePressure { get; set; }
        public double avgValePressure { get; set; }

        public double avgOfAvailableHoursPressure { get; set; }
        public double avgOfNonAvailableHoursPressure { get; set; }

        public double minValueManual { get; set; }
        public double maxValueManual { get; set; }
        public double avgValeManual { get; set; }

        public double avgOfAvailableHoursManual { get; set; }
        public double avgOfNonAvailableHoursManual { get; set; }

        public double minValueIndoorLight { get; set; }
        public double maxValueIndoorLight { get; set; }
        public double avgValeIndoorLight { get; set; }

        public double avgOfAvailableHoursIndoorLight { get; set; }
        public double avgOfNonAvailableHoursIndoorLight { get; set; }

        public double minValueOutdoorLight { get; set; }
        public double maxValueOutdoorLight { get; set; }
        public double avgValeOutdoorLight { get; set; }

        public double avgOfAvailableHoursOutdoorLight { get; set; }
        public double avgOfNonAvailableHoursOutdoorLight { get; set; }

        public double minValueExhaust { get; set; }
        public double maxValueExhaust { get; set; }
        public double avgValeExhaust { get; set; }

        public double avgOfAvailableHoursExhaust { get; set; }
        public double avgOfNonAvailableHoursExhaust { get; set; }

        public double minValuevib_v { get; set; }
        public double maxValuevib_v { get; set; }
        public double avgValevib_v  { get; set; }

        public double avgOfAvailableHoursvib_v { get; set; }
        public double avgOfNonAvailableHoursvib_v { get; set; }

        public double minValuevib_a { get; set; }
        public double maxValuevib_a { get; set; }
        public double avgValevib_a { get; set; }

        public double avgOfAvailableHoursvib_a { get; set; }
        public double avgOfNonAvailableHoursvib_a { get; set; }

        public double minValuevib_d { get; set; }
        public double maxValuevib_d { get; set; }
        public double avgValevib_d { get; set; }

        public double avgOfAvailableHoursvib_d { get; set; }
        public double avgOfNonAvailableHoursvib_d { get; set; }



    }
}