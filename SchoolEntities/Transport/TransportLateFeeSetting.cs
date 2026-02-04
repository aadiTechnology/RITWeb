// -----------------------------------------------------------------------
// <copyright file="TransportLateFeeSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransportEntities
{
    //This entity class use to get late fee settings 
    public class TransportLateFeeDueDate 
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public string Month { get; set; }
    }
    //Set/Get late fee value
    public class TransportLateFeeSetting
    {
        public int LateFeeAmount { get; set; }
        public int LateFeePerTypeId { get; set; }
        public int ValueForType { get; set; }
        public int InsertedById { get; set; }
        public DateTime TransportStartDate { get; set; }
        public DateTime TransportEndDate { get; set; }
    }
  

}
