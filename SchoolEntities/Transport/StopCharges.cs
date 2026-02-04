// -----------------------------------------------------------------------
// <copyright file="StopCharges.cs" company="Microsoft">
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
    using MasterEntities;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StopCharge
    {
        public int StopId { get; set; }
        public string Charges { get; set; }
        public string OneWayCharges { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }   
    }
}
