using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WASA_EMS
{
    public class RangeAndResourceSelector
    {
        public string resourceType { get; set; }
        public string resourceName { get; set; }
        public string dateFrom { get; set; }
        public string timeFrom { get; set; }
        public string dateTo { get; set; }
        public string timeTo { get; set; }
    }

    public class ResourceAndParameterSelector
    {
        public string resourceID { get; set; }
        public string parameterID { get; set; }
        public string dateFrom { get; set; }
        public string timeFrom { get; set; }
        public string dateTo { get; set; }
        public string timeTo { get; set; }
    }
}