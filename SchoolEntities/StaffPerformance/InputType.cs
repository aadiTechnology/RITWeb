// -----------------------------------------------------------------------
// file="InputType.cs" 
// Created By = Yogesh
// Date = 27 Mar 2015
// -----------------------------------------------------------------------

using System;
using SchoolEntities;

namespace StaffPerformanceEntity
{
    [Serializable]
    public class InputType : SchoolEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
