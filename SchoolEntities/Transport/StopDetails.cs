// -----------------------------------------------------------------------
// <copyright file="StopDetails.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SchoolEntities.Transport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SchoolEntities;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StopDetails
    {
        public int StopId { get; set; }
        public string StopName { get; set; }
        public string Charges { get; set; }
        public string OneWayCharges { get; set; }
        public List<StopCharge> lstStopCharges = new List<StopCharge>();
       
    }
}
