using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WASA_EMS
{
    public class WhatsAppDaily
    {
        public int tubewells { get; set; }
        public int activeTubewells { get; set; }
        public int inactiveTubewells { get; set; }
        public int disposals { get; set; }
        public int activeDisposals { get; set; }
        public int inactiveDisposals { get; set; }
        public int recyclingPlants { get; set; }
        public int activeRrecyclingPlants { get; set; }
        public int inactiveRrecyclingPlant { get; set; }
        public IList<TubewellsWorkingHoursList> tubewellsWorkingHoursLists { get; set; }
        public IList<DisposalWorkingHoursList> disposalWorkingHoursLists { get; set; }
        public IList<RecyclingPlantWorkingHoursList> recyclingPlantWorkingHoursLists { get; set; }
    }
}